using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckoutSystem.Core.Interfaces;
using CheckoutSystem.Core.Models;

public class CheckoutService : ICheckoutService
{
    private readonly IPricingService _pricingService;
    private readonly List<Item> _items;
    private readonly IItemRepository _itemRepository;

    public CheckoutService(IPricingService pricingService, IItemRepository itemRepository)
    {
        _pricingService = pricingService;
        _items = new List<Item>();
        _itemRepository = itemRepository;
    }

    public void Scan(string sku)
    {
        
       var item = _itemRepository.GetItemBySku(sku);
               
      _items.Add(item);
        
    }

    public decimal GetTotal()
    {
        return _pricingService.CalculatePrice(_items);
    }
}