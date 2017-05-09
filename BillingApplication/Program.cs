using PricingEngine.Factories;
using PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingEngine.Extensions;

namespace BillingApplication
{

    class Program
    {
        public static string Ask(string statement)
        {
            string input;
            Console.WriteLine(statement);
            input = Console.ReadLine();
            return input;

        }

        static void Main(string[] args)
        {
            while (Ask("Do you want to generate a Bill (Y/N)").ToUpper() == "Y")
            {
                Proceed();
            }
        }

        private static void DisplayBillDetails(IBill bill)
        {
            decimal totalamount = bill.GetTotalAmount();
            decimal amountAfterDiscount = bill.GetTotalAmountAfterDiscount();
            decimal finalpayment = bill.GetFinalPayAmount();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine(String.Format("Customer Name : {0}", bill.Customer.Name));
            Console.WriteLine(String.Format("Customer Type : {0}", bill.Customer.TypeofCustomer.ToString()));
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("                      Bill Details                                              ");
            Console.WriteLine("--------------------------------------------------------------------------------");

            Extensions.ForEach<LineItem>(
                bill.Items,
                (item => Console.WriteLine(String.Format("Item Name : {0}      Price : {1}     Price after Discount  : {2}", item.Name, item.Price.actualCost.ToString(), item.Price.CostAfterDiscount.ToString())))
           );
            Console.WriteLine(String.Format("Total Original Amount : {0}", totalamount));
            Console.WriteLine(String.Format("Total Amount after Discount on items : {0}", amountAfterDiscount));
            Console.WriteLine(String.Format("Total Amount to be paid after discount on Bill : {0}", finalpayment));
            Console.WriteLine("--------------------------------------------------------------------------------");

        }

        private static void Proceed()
        {
            var customername = Ask("Enter customer name  : ");

            CustomerType customertype;
            DateTime registereddate;
            List<LineItem> cart = new List<LineItem>();
            ICustomer customer;
            IBill bill;


            int countOfItems = 0;
            int counter = 0;

            while (!Enum.TryParse(Ask("enter customer type :  Standard (0) / Employee (1) / Affiliate (2)"), true, out customertype))
            {
                Console.Write("Enter valid  customer type  as integer \n");
            }

            while (!DateTime.TryParseExact(Ask("enter registrationd date [dd/MM/yyyy]: "), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out registereddate))
            {
                Console.Write("enter valid  Registration  date  in  format [dd/MM/yyyy] \n");
            }

            while (!Int32.TryParse(Ask("enter No of Items you want to buy : "), out countOfItems))
            {
                Console.Write("enter number of Items you want to buy as integer \n");
            }

            while (counter < countOfItems)
            {
                string itemname;
                ItemType itemtype;
                decimal itemcost;
                LineItem item;

                itemname = Ask(String.Format("Enter Name of  Item {0}  : ", (counter + 1).ToString()));
                while (!Enum.TryParse(Ask("enter Item type :  Grocery (0) / Others (1) "), true, out itemtype))
                {
                    Console.Write("Please enter a valid Item type: $");
                }

                while (!decimal.TryParse(Ask("enter cost of item :"), out itemcost))
                {
                    Console.Write("Please enter a valid cost of Item as decimal :");
                }

                item = ItemFactory.CreateItem(new Tuple<string, ItemType, decimal>(itemname, itemtype, itemcost));
                cart.Add(item);
                counter += 1;

            }

            customer = CustomerFactory.GetCustomer(new Tuple<string, DateTime, CustomerType>(customername, registereddate, customertype));
            bill = BillFactory.GenerateBill(customer, cart);
            DisplayBillDetails(bill);


        }
    }
}
