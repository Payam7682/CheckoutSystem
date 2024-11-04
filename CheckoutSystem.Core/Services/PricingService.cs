using CheckoutSystem.Core.Interfaces;
using CheckoutSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutSystem.Core.Services
{
    public class PricingService : IPricingService
    {
        private readonly List<PricingRule> _pricingRules;

        public PricingService(List<PricingRule> priceRules)
        {
            _pricingRules = priceRules;
        }

        public decimal CalculatePrice(List<Item> items)
        {
            decimal total = 0;
            var groupItems = items.GroupBy(item => item.SKU);

            foreach (var group in groupItems)
            {
                var sku = group.Key;
                var quantity = group.Count();
                var item = group.First();
                var rule = _pricingRules.FirstOrDefault(r => r.SKU == sku);
                // Apply "Buy One, Get One Free" offer for item 'C'
                //if (sku == "C")
                //{
                //    int payableQuantity = (quantity / 2) + (quantity % 2); // Half of the items are free
                //    total += payableQuantity * group.First().UnitPrice;
                //}
                //else
                if (rule != null && quantity >= rule.Quantity)
                {
                    int discountedSets = quantity / rule.Quantity;
                    int remainingItems = quantity % rule.Quantity;

                    total += discountedSets * rule.SpecialPricer + remainingItems * item.UnitPrice;
                }
                else
                {
                    total += quantity * item.UnitPrice;
                }

            }
           
            return total;
        }


    }
}
