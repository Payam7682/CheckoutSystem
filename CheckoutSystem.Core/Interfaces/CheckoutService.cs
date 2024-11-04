using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckoutSystem.Core.Models;

namespace CheckoutSystem.Core.Interfaces
{
    public interface ICheckoutService
    {
        void Scan(string sku);
        decimal GetTotal();
    }
}
