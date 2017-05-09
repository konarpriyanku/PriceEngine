using PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Factories
{
    public class BillFactory
    {
        public static IBill GenerateBill(Customer customer, IEnumerable<LineItem> items)
        {
            var bill = new Bill
            {
                Customer = customer,
                Items = items
            };
            return bill;

        }
    }
}
