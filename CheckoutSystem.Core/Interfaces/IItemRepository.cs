using CheckoutSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutSystem.Core.Interfaces
{
    public interface IItemRepository
    {
        Item GetItemBySku(string sku);
    }
}
