using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Models
{
    public class CartModels 
    {
        public class Cart
        {
            public string Id { get; set; }

            public List<Item> Items { get; set; }
        }

        public class Item
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Category { get; set; }

            public float Price { get; set; }
        }
    }
}
