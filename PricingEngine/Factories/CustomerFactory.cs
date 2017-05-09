using PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Factories
{
   public static class CustomerFactory
    {
        //Factory to create a Customer based on customer meta data 
        public static Customer GetCustomer(Tuple<string,DateTime,CustomerType> customermetadata)
        {
            return new Customer
            {
                Name = customermetadata.Item1,
                RegisteredOn = customermetadata.Item2,
                TypeofCustomer = customermetadata.Item3

            };

        }
    }
}
