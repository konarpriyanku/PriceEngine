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
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

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

        public static Decimal ApplyDiscount(this IBill bill)
        {
            var finalamount = bill.GetTotalAmountAfterDiscount();
          if (finalamount > 0  && finalamount >= 100)
            {
                finalamount = finalamount - (5 * (Math.Floor(finalamount / 100M)));
            }
          return finalamount;


        }

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
