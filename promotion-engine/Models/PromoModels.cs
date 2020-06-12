using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Models
{
    public class PromoModels
    {
        public class MarkedItem
        {
            public CartModels.Item Item { get; set; }

            public Dictionary<string, bool> MarkedBuys { get; set; }

            public Dictionary<string, float> MarkedGets { get; set; }
        }

        public class PromofiedCart
        {
            public float TotalPrice { get; set; }

            public float TotalOffPrice { get; set; }

            public List<MarkedItem> Items { get; set; }

        }

        public class Promo
        {
            public string Id { get; set; }

            public string Description { get; set; }

            public List<Buy> Buys { get; set; }

            public List<Get> Gets { get; set; }

        }

        public class Get
        {
            public string Category { get; set; }

            public bool All { get; set; }

            public int Count { get; set; }

            public Off Off { get; set; }

        }

        public class Buy
        {
            public string Category { get; set; }

            public int Count { get; set; }
        }

        public class Fixed
        {
            public float Price { get; set; }
        }

        public class Off
        {
            public Discount Discount { get; set; }

            public Fixed Fixed { get; set; }
        }

        public class Discount
        {
            public float Percentage { get; set; }
        }
    }
}
