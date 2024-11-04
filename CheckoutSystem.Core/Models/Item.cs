using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutSystem.Core.Models
{
    public class Item
    {
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }

        public Item( string sku, decimal unitPrice )
        {
            SKU = sku;
            UnitPrice = unitPrice;
        }
    }
}
