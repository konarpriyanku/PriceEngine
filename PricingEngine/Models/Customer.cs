using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Models
{

    public interface ICustomer
    {
        string Name { get; set; }
        CustomerType TypeofCustomer { get; set; }
        DateTime RegisteredOn { get; set; }
    }

    public enum CustomerType
    {
        Standard,
        Employee,
        Affiliate
    }


    public  class Customer : ICustomer
    {
       public  string Name { get; set; }
       public  CustomerType TypeofCustomer { get; set; }
       public DateTime RegisteredOn { get; set; }
    }
}
