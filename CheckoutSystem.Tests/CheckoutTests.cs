using CheckoutSystem.Core.Interfaces;
using CheckoutSystem.Core.Models;
using CheckoutSystem.Core.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CheckoutSystem.Tests
{
    public class CheckoutTests
    {
        private readonly ICheckoutService _checkoutService;
        private readonly Mock<IPricingService> _pricingServiceMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;

        public CheckoutTests()
        {
            // Set up the mock for IItemRepository
            _itemRepositoryMock = new Mock<IItemRepository>();
            _itemRepositoryMock.Setup(repo => repo.GetItemBySku("A")).Returns(new Item("A", 50));
            _itemRepositoryMock.Setup(repo => repo.GetItemBySku("B")).Returns(new Item("B", 30));
            _itemRepositoryMock.Setup(repo => repo.GetItemBySku("C")).Returns(new Item("C", 20));
            _itemRepositoryMock.Setup(repo => repo.GetItemBySku("D")).Returns(new Item("D", 15));
           

            _pricingServiceMock = new Mock<IPricingService>();

            // Set up initial mock behavior for pricing service
            _pricingServiceMock.Setup(service => service.CalculatePrice(It.IsAny<List<Item>>()))
                .Returns((List<Item> items) =>
                {
                    if (items == null) return 0;  // Handle null input
                    int total = 0;
                    var itemGroups = new Dictionary<string, int>();

                    foreach (var item in items)
                    {
                        if (!itemGroups.ContainsKey(item.SKU))
                        {
                            itemGroups[item.SKU] = 0;
                        }
                        itemGroups[item.SKU]++;
                    }

                    // Custom logic for mocked pricing
                    foreach (var group in itemGroups)
                    {
                        switch (group.Key)
                        {
                            case "A":
                                total += (group.Value / 3) * 130 + (group.Value % 3) * 50;
                                break;
                            case "B":
                                total += (group.Value / 2) * 45 + (group.Value % 2) * 30;
                                break;
                            case "C":
                                total += group.Value * 20;
                                break;
                            case "D":
                                total += group.Value * 15;
                                break;
                            
                        }
                    }

                    

                    return total;
                });

            _checkoutService = new CheckoutService(_pricingServiceMock.Object, _itemRepositoryMock.Object);
        }

        [Theory]
        [InlineData("", 0)] // No items scanned
        [InlineData("A", 50)] // Single valid item
        [InlineData("AB", 80)] // Two different valid items
        [InlineData("CDBA", 115)] // Four different valid items
        [InlineData("AA", 100)] // Two of the same item
        [InlineData("AAA", 130)] // Three of the same item to trigger special pricing
        [InlineData("AAAA", 180)] // Four of the same item
        [InlineData("AAAAA", 230)] // Five of the same item
        [InlineData("AAAAAA", 260)] // Six of the same item to apply special pricing twice
        [InlineData("AAAB", 160)] // Combination to trigger special pricing and regular pricing
        [InlineData("AAABB", 175)] // Combination to trigger special pricing for both items
        [InlineData("AAABBD", 190)] // Combination with multiple discounts
        [InlineData("DABABA", 190)] // Random order of items
        [InlineData("AAAAAAAAAAABBBBBBBBBBB", 745)] // Large quantity to check multiple special pricings       
        public void TestTotals(string items, int expectedTotal)
        {
            // Arrange
            var total = Price(items);

            // Assert
            Assert.Equal(expectedTotal, total);
        }

        [Theory]
        [InlineData(new string[] { }, 0)] // No items scanned
        [InlineData(new string[] { "A" }, 50)] // Single item scanned
        [InlineData(new string[] { "A", "B" }, 80)] // Two different items scanned
        [InlineData(new string[] { "A", "A", "A" }, 130)] // Three of the same item scanned
        [InlineData(new string[] { "A", "A", "A", "B" }, 160)] // Mixed scan triggering special pricing
        [InlineData(new string[] { "A", "A", "A", "B", "B" }, 175)] // Special pricing for both A and B        
        public void TestIncremental(string[] items, int expectedTotal)
        {
            // Act
            foreach (var item in items)
            {
                _checkoutService.Scan(item);
            }

            decimal total = _checkoutService.GetTotal();

            // Assert
            Assert.Equal(expectedTotal, total);
        }
      

        private decimal Price(string goods)
        {
            foreach (var item in goods)
            {
                _checkoutService.Scan(item.ToString());
            }
            return _checkoutService.GetTotal();
        }
    }
}
