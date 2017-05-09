using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Strategy
{
    //Strategy pattern  to solve the problem 
    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal price);
    }

    public class NoDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price)
        {
            return price;
        }
    }

    public class EmployeeDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price)
        {
            return price  * 0.70M;
        }
    }

    public class AffiliateDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price)
        {
            return price * 0.9M;
        }
    }

    public class OldCustomerDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal price)
        {
            return price * 0.95M;
        }
    }
}
