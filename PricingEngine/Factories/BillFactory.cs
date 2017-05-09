using PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Factories
{
    //factory for creatting a Bill  for a Customer with a  list of cart items 
    public class BillFactory
    {
        public static IBill GenerateBill(ICustomer customer, IEnumerable<LineItem> items)
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
