using PricingEngine.Models;
using PricingEngine.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingEngine.Extensions;

namespace PricingEngine.Models
{
    public interface IBill
    {
        ICustomer Customer{ get; set; }
        IEnumerable<LineItem> Items { get; set; }
        Decimal GetTotalAmount();
        Decimal GetTotalAmountAfterDiscount();
        Decimal GetFinalPayAmount();
    }

    public class Price
    {
        IDiscountStrategy _discountStrategy;
        public decimal actualCost { get; set; }
        public decimal CostAfterDiscount { get; set; }

        public Price()
        {
            _discountStrategy = new NoDiscount();
        }

    }

    public enum ItemType
    {
        Grocery,
        Others
    }

    public class LineItem
    {
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
        public Price Price { get; set; }
    }

    public class Bill :IBill
    {

       public  ICustomer Customer{ get; set; }
       public  IEnumerable<LineItem> Items { get; set; }

        public Decimal GetFinalPayAmount()
        {
           return  this.ApplyDiscount();

        }
        public Decimal GetTotalAmount()
        {
            decimal totalcost = 0;
            if (this.Items.ToList().Count > 0)
            {
                Extensions.Extensions.ForEach<LineItem>(this.Items, (item => totalcost += item.Price.actualCost));
            }
     
            return totalcost;
        }
        public Decimal GetTotalAmountAfterDiscount()
        {
            decimal discountedcost = 0;
            if (this.Items.ToList().Count > 0)
            {
                Extensions.Extensions.ForEach<LineItem>(this.Items, (item=> {
                    item.ApplyDiscount(this.Customer.GetDiscountStrategy());
                    discountedcost += item.Price.CostAfterDiscount;
                }));
            }
          
            return discountedcost;
        }

    



    }
}
