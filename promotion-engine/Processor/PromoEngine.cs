using Microsoft.CodeAnalysis.CSharp.Syntax;
using promotion_engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace promotion_engine.Processor
{
    public class PromoEngine : IPromoEngine
    {
        // Implement the calculation process for promo engine
        PromoModels.PromofiedCart IPromoEngine.Process(List<PromoModels.Promo> promos, CartModels.Cart cart)
        {
            var groupedItems = new Dictionary<string, List<PromoModels.MarkedItem>>();

            foreach (var item in cart.Items)
            {
                if (!groupedItems.ContainsKey(item.Category))
                {
                    groupedItems.Add(item.Category, new List<PromoModels.MarkedItem> { new PromoModels.MarkedItem { Item = item, MarkedBuys = new Dictionary<string, bool>(), MarkedGets = new Dictionary<string, float>() } });
                }
                else
                {
                    groupedItems[item.Category].Add(new PromoModels.MarkedItem { Item = item, MarkedBuys = new Dictionary<string, bool>(), MarkedGets = new Dictionary<string, float>() });
                }
            }

            foreach (var promo in promos)
            {
                groupedItems = ApplyPromo(promo, groupedItems);
            }

            var items = new List<PromoModels.MarkedItem>();

            foreach (var item in groupedItems)
            {
                items.AddRange(item.Value);
            }

            var prices = ComputePrices(items);

            var totalPrice = prices[0];
            var totalOffPrice = prices[1];

            PromoModels.PromofiedCart promofiedCart = new PromoModels.PromofiedCart { Items = items, TotalPrice = totalPrice, TotalOffPrice = totalOffPrice };
            return promofiedCart;
        }


        private Dictionary<string, List<PromoModels.MarkedItem>> MarkBuyItems(Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Buy buy, PromoModels.Promo promo)
        {
            var markCount = 0;

            for (int i = 0; i < groupedItems[buy.Category].Count; i++)
                {
                    var item = groupedItems[buy.Category][i];

                if (markCount == buy.Count)
                {
                    break;
                }

                var isMarkedBuy = item.MarkedBuys.TryGetValue(promo.Id, out bool val1);
                var isMarkedGet = item.MarkedGets.TryGetValue(promo.Id, out float val2);

                if (!isMarkedBuy && !isMarkedGet)
                {
                    groupedItems[buy.Category][i].MarkedBuys[promo.Id] = true;
                    markCount += 1;
                }
            }

            return groupedItems;
        }

        private AppliedBuy ApplyBuy(PromoModels.Buy buy, Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Promo promo)
        {
            if (!groupedItems.TryGetValue(buy.Category, out List<PromoModels.MarkedItem> value))
            {
                return new AppliedBuy{ Applied = false, GroupItems = groupedItems };
            }

            var matchedItems = new List<PromoModels.MarkedItem>();

            foreach (var item in groupedItems[buy.Category])
            {
                var isMarkedBuy = item.MarkedBuys.TryGetValue(promo.Id, out bool val1);
                var isMarkedGet = item.MarkedGets.TryGetValue(promo.Id, out float val2);
                
                if (!isMarkedBuy && !isMarkedGet)
                {
                    matchedItems.Add(item);
                }
            }

            if(matchedItems.Count < buy.Count)
            {
                return new AppliedBuy{ Applied = false, GroupItems = groupedItems };
            }

            groupedItems = MarkBuyItems(groupedItems, buy, promo);

            return new AppliedBuy{ Applied = true, GroupItems = groupedItems };
        }

        private AppliedBuys ApplyBuys(Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Promo promo)
        {
            var applied = true;

            foreach (var buy in promo.Buys)
            {
                var appliedBuy = ApplyBuy(buy, groupedItems, promo);
                applied = applied && appliedBuy.Applied;
            }

            return new AppliedBuys { Applied = applied, GroupItems = groupedItems };
        }

        private float ComputeOffPrice(float price, PromoModels.Off off)
        {
            if(off.Discount != null)
            {
                return price - (price * off.Discount.Percentage / 100);
            }
            else
            {
                return off.Fixed.Price;
            }
        }

        private Dictionary<string, List<PromoModels.MarkedItem>> MarkGetItems(Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Get get, PromoModels.Promo promo)
        {
            var markCount = 0;

            for (int i = 0; i < groupedItems[get.Category].Count; i++)
            {
                var item = groupedItems[get.Category][i];

                if (get.All == false && markCount == get.Count)
                {
                    break;
                }

                var isMarkedBuy = item.MarkedBuys.TryGetValue(promo.Id, out bool val1);
                var isMarkedGet = item.MarkedGets.TryGetValue(promo.Id, out float val2);

                if (!isMarkedBuy && !isMarkedGet)
                {
                    groupedItems[get.Category][i].MarkedGets[promo.Id] = ComputeOffPrice(item.Item.Price, get.Off);
                    markCount += 1;
                }
            }

            return groupedItems;
        }
    
        private Dictionary<string, List<PromoModels.MarkedItem>> ApplyGet(PromoModels.Get get, Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Promo promo)
        {
            if (!groupedItems.TryGetValue(get.Category, out List<PromoModels.MarkedItem> val))
            {
                return groupedItems;
            }

            groupedItems = MarkGetItems(groupedItems, get, promo);

            return groupedItems;
        }

        private Dictionary<string, List<PromoModels.MarkedItem>> ApplyGets(Dictionary<string, List<PromoModels.MarkedItem>> groupedItems, PromoModels.Promo promo)
        {
            foreach (var get in promo.Gets)
            {
                groupedItems = ApplyGet(get, groupedItems, promo);
            }

            return groupedItems;
        }

        private Dictionary<string, List<PromoModels.MarkedItem>> ApplyPromo(PromoModels.Promo promo, Dictionary<string, List<PromoModels.MarkedItem>> groupedItems)
        {
            do
            {            
                var appliedBuys = ApplyBuys(groupedItems, promo);

                if (!appliedBuys.Applied)
                {
                    break;
                }

                groupedItems = appliedBuys.GroupItems;
                groupedItems = ApplyGets(groupedItems, promo);

            } while (true);

            return groupedItems;
        }

        private float GetMinOffPrice(float originalPrice, PromoModels.MarkedItem markedItem)
        {
            if (markedItem.MarkedGets.Count == 0)
            {
                return originalPrice;
            }

            var minPrice = originalPrice;           

            foreach (var offPrice in markedItem.MarkedGets)
            {
                if (minPrice > offPrice.Value)
                {
                    minPrice = offPrice.Value;
                }
            }
            return minPrice;
        }

        private List<float> ComputePrices(List<PromoModels.MarkedItem> items)
        {
            float totalPrice = 0;
            float totalOffPrice = 0;

            foreach (var markedItem in items)
            {
                totalPrice = totalPrice + markedItem.Item.Price;
                // if multiple promos apply, take the one with the lowest price
                totalOffPrice = totalOffPrice + GetMinOffPrice(markedItem.Item.Price, markedItem);
            }

            return new List<float> { totalPrice, totalOffPrice };
        }
    }



    public class AppliedBuys
    {
        public bool Applied { get; set; }
        public Dictionary<string, List<PromoModels.MarkedItem>> GroupItems { get; set; }
    }

    public class AppliedBuy
    {
        public bool Applied { get; set; }
        public Dictionary<string, List<PromoModels.MarkedItem>> GroupItems { get; set; }
    }

}
