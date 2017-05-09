using PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Factories
{
    //factory to create a LineItem 
    public static class ItemFactory
    {
      
        public static LineItem CreateItem(Tuple<string,ItemType,decimal> itemmetadata)
        {

            return new LineItem
            {
                Name = itemmetadata.Item1,
                ItemType = itemmetadata.Item2,
                Price = new Price { actualCost = itemmetadata.Item3 }
            };

        }
    }
}
