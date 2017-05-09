using NUnit.Framework;
using PricingEngine.Factories;
using PricingEngine.Models;
using PricingEngine.Extensions;
using PricingEngine.Strategy;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace PriceEngineTest
{
    [TestFixture]
    public class DiscountTest
    {
        public IEnumerable<LineItem> items;
       
        [SetUp]
        public void Init()
        {
            items = new List<LineItem>()
            {
                new LineItem { Name = "Item 1", ItemType = ItemType.Others,Price = new Price { actualCost = 100} },
                new LineItem { Name = "Item 2", ItemType = ItemType.Others,Price = new Price { actualCost = 100} },
                new LineItem { Name = "Item 3", ItemType = ItemType.Grocery,Price = new Price { actualCost = 100} }
               
            };

        }

        private Customer CreateCustomer(string name, string registeredon, CustomerType custtype)
        {
            DateTime registereddate;
            DateTime.TryParseExact(registeredon, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out registereddate);
            Customer customer = CustomerFactory.GetCustomer(new Tuple<string, DateTime, CustomerType>(name, registereddate, custtype));
            return customer;
        }

        private IBill CalculateBill(string name, CustomerType custtype, string registeredon, string itemname, ItemType itemtype, decimal cost)
        {
            var customer = CreateCustomer(name, registeredon, custtype);
            List<LineItem> itemlist = new List<LineItem>();
            itemlist.Add(ItemFactory.CreateItem(new Tuple<string, ItemType, decimal>(itemname, itemtype, cost)));
            IBill bill = BillFactory.GenerateBill(customer, itemlist);
            return bill;
        }

        private IBill CalculateBill(string name, CustomerType custtype, string registeredon, IEnumerable<LineItem> itemlist)
        {
            var customer = CreateCustomer(name, registeredon, custtype);
            IBill bill = BillFactory.GenerateBill(customer, itemlist);
            return bill;
        }

        [TestCase ("Customer1" ,CustomerType.Standard,"08/05/2016")]
        [TestCase("Customer2", CustomerType.Standard, "08/05/2014")]
        [TestCase("Customer3", CustomerType.Employee, "08/05/2014")]
        [TestCase("Customer4", CustomerType.Affiliate, "08/05/2014")]
        public void Test_DiscountStrategy_Based_On_CustomerType(string name, CustomerType custtype,string registeredon  )
        {

            var customer = CreateCustomer(name, registeredon, custtype);
            IDiscountStrategy strategy = customer.GetDiscountStrategy();

            switch (name)
            {
                case "Customer1":
                    Assert.That(strategy is NoDiscount);
                    break;
                case "Customer2":
                    Assert.That(strategy is OldCustomerDiscount);
                    break;
                case "Customer3":
                    Assert.That(strategy is EmployeeDiscount);
                    break;
                case "Customer4":
                    Assert.That(strategy is AffiliateDiscount);
                    break;
                default:
                    Assert.That(strategy is NoDiscount);
                    break;
            }
            

        }

        [TestCase("Customer1", CustomerType.Employee, "08/05/2016","ItemName",ItemType.Others,300)]
        [TestCase("Customer1", CustomerType.Affiliate, "08/05/2016", "ItemName", ItemType.Others, 300)]
        [TestCase("Customer1", CustomerType.Standard, "08/05/2013", "ItemName", ItemType.Others, 300)]
        public void Test_Discount_Based_On_CustomerType(string name, CustomerType custtype, string registeredon,string itemname, ItemType itemtype, decimal cost)
        {
            Bill bill = (Bill)CalculateBill(name, custtype, registeredon, itemname, itemtype, cost);


            switch (custtype)
            {
                case CustomerType.Employee:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(210, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(200, bill.GetFinalPayAmount());
                    break;
                case CustomerType.Affiliate:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(270, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(260, bill.GetFinalPayAmount());
                    break;
                case CustomerType.Standard:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(285, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(275, bill.GetFinalPayAmount());
                    break;
            }

        }

        [TestCase("Customer1", CustomerType.Employee, "08/05/2016", "ItemName", ItemType.Grocery, 300)]
        [TestCase("Customer2", CustomerType.Employee, "08/05/2016", "ItemName", ItemType.Others, 300)]
        public void Test_Discount_Based_On_ItemType(string name, CustomerType custtype, string registeredon, string itemname, ItemType itemtype, decimal cost)
        {
            Bill bill = (Bill)CalculateBill(name, custtype, registeredon, itemname, itemtype, cost);

            switch (itemtype)
            {
                case ItemType.Grocery:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(300, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(285, bill.GetFinalPayAmount());
                    break;
                case ItemType.Others:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(210, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(200, bill.GetFinalPayAmount());
                    break;
            }
        }

        [TestCase("Customer1", CustomerType.Standard, "08/05/2016", "ItemName", ItemType.Others, 300)]
        [TestCase("Customer2", CustomerType.Standard, "08/05/2014", "ItemName", ItemType.Others, 300)]
        public void Test_Discount_Based_On_dateofregistration(string name, CustomerType custtype, string registeredon, string itemname, ItemType itemtype, decimal cost)
        {
            Bill bill = (Bill)CalculateBill(name, custtype, registeredon, itemname, itemtype, cost);

            switch (registeredon)
            {
                case "08/05/2016":
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(300, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(285, bill.GetFinalPayAmount());
                    break;
                case "08/05/2014":
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(285, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(275, bill.GetFinalPayAmount());
                    break;
            }

        }


        [TestCase("Customer1", CustomerType.Standard, "08/05/2016")]
        [TestCase("Customer2", CustomerType.Standard, "08/05/2014")]
        public void Test_Discount_MultipleItems_Based_On_registeredon(string name, CustomerType custtype, string registeredon)
        {
            Bill bill = (Bill)CalculateBill(name, custtype, registeredon,items);

            switch (registeredon)
            {
                case "08/05/2016":
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(300, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(285, bill.GetFinalPayAmount());
                    break;
                case "08/05/2014":
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(290, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(280, bill.GetFinalPayAmount());
                    break;
            }

        }

        [TestCase("Customer1", CustomerType.Employee, "08/05/2016" )]
        [TestCase("Customer1", CustomerType.Affiliate, "08/05/2016")]
        [TestCase("Customer1", CustomerType.Standard, "08/05/2016")]
        public void Test_Discount_MultipleItems_Based_On_CustomerType(string name, CustomerType custtype, string registeredon)
        {
            Bill bill = (Bill)CalculateBill(name, custtype, registeredon, items);


            switch (custtype)
            {
                case CustomerType.Employee:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(240, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(230, bill.GetFinalPayAmount());
                    break;
                case CustomerType.Affiliate:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(280, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(270, bill.GetFinalPayAmount());
                    break;
                case CustomerType.Standard:
                    Assert.AreEqual(300, bill.GetTotalAmount());
                    Assert.AreEqual(300, bill.GetTotalAmountAfterDiscount());
                    Assert.AreEqual(285, bill.GetFinalPayAmount());
                    break;
            }

        }

        [TearDown]
       public void Close()
        {
            items = null;
        }
    }
}

