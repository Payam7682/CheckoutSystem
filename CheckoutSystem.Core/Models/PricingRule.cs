using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutSystem.Core.Models
{
    public class PricingRule
    {
        public string SKU { get; set; }        
        public int Quantity { get; set; }
        public decimal SpecialPricer { get; set; }

        public PricingRule(string sku, int quantity, decimal specialPrice)
        {
            SKU = sku;
            Quantity = quantity;
            SpecialPricer = specialPrice;
        
        }
    }
}
