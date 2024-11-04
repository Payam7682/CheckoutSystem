using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckoutSystem.Core.Models;

namespace CheckoutSystem.Infrastructure.Repositories
{
    public class PricingRepository
    {
        public List<PricingRule> GetPricingRules()
        {
            return new List<PricingRule>
            {
                new PricingRule("A",3,130),
                new PricingRule("B",2,45)            

            };
        }
    }
}
