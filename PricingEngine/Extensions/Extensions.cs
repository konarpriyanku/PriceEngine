using PricingEngine.Models;
using PricingEngine.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Extensions
{
  
   public static class Extensions
    {
        //wrapper for for each  loop used with any ienumerable source 
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        // the discount to be applied on each lineitem based on the discount strategy
        public static void ApplyDiscount(this LineItem item, IDiscountStrategy discountstrategy )
        {
            if (item.ItemType != ItemType.Grocery)
            {
                item.Price.CostAfterDiscount = discountstrategy.ApplyDiscount(item.Price.actualCost);
            }
            else
            {
                item.Price.CostAfterDiscount = item.Price.actualCost;
            }

        }

        // the discount to be applied on the final bill based on the total bill amount
        public static Decimal ApplyDiscount(this IBill bill)
        {
            var finalamount = bill.GetTotalAmountAfterDiscount();
          if (finalamount > 0  && finalamount >= 100)
            {
                finalamount = finalamount - (5 * (Math.Floor(finalamount / 100M)));
            }
          return finalamount;


        }

        // derive the discount based on customer type 
        public static IDiscountStrategy GetDiscountStrategy(this ICustomer customer)
        {
            IDiscountStrategy discountstrategy;
            int timeSinceCustomer = (DateTime.MinValue + (DateTime.Now - customer.RegisteredOn)).Year - 1;
                
            if (customer.TypeofCustomer == CustomerType.Employee)
            {
                discountstrategy=new EmployeeDiscount();
            }
            else if (customer.TypeofCustomer == CustomerType.Affiliate)
            {
                discountstrategy= new AffiliateDiscount();
            }
            else if(timeSinceCustomer > 2)
            {
                discountstrategy= new OldCustomerDiscount();
            }
            else
            {
                discountstrategy= new NoDiscount();
            }
            return discountstrategy;
        }

    }
}
