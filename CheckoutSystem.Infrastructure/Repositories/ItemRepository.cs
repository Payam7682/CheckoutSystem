using CheckoutSystem.Core.Interfaces;
using CheckoutSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutSystem.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly Dictionary<string, Item> _items = new Dictionary<string, Item>
        {
            { "A", new Item("A", 50) },
            { "B", new Item("B", 30) },
            { "C", new Item("C", 20) },
            { "D", new Item("D", 15.5m) }
        };

        public Item GetItemBySku(string sku)
        {

            if (!_items.TryGetValue(sku, out var item))
            {
                  return new Item(sku, 0);
            }
            return item;
        }
    }
}
