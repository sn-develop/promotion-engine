using promotion_engine.Models;
using promotion_engine.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace promotion_engine.Repository
{
    public class CartRepo : ICartRepo
    {
        List<CartModels.Cart> inMemoryCartList = new List<CartModels.Cart>();

        private readonly IPromoRepo _promoRepo;
        private readonly IPromoEngine _promoEngine;

        public CartRepo(IPromoRepo promoRepo, IPromoEngine promoEngine)
        {
            _promoRepo = promoRepo;
            _promoEngine = promoEngine;
        }

        public string AddCart(CartModels.Cart cart)
        {
            if (cart == null)
            {
                throw new Exception("Not a valid cart");
            }

            if (string.IsNullOrEmpty(cart.Id))
                cart.Id = new Guid().ToString();

            inMemoryCartList.Add(cart);
            return cart.Id;
        }

        public bool DeleteCart(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Not a valid cart id");
            }

            inMemoryCartList = inMemoryCartList.Where(cart => cart.Id != id).ToList();
            // Save list

            return true;
        }

        public CartModels.Cart GetCart(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Not a valid cart id");
            }

            var cart = inMemoryCartList.Where(cart => cart.Id != id).FirstOrDefault();
            return cart;
        }

        public PromoModels.PromofiedCart ApplyPromo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Not a valid cart id");
            }

            CartModels.Cart cart = GetCart(id);

            // assuming that promo code ids are sent in the controller request
            var promoCodes = new List<string>();

            var promoCollection = new List<PromoModels.Promo>();

            foreach (var pc in promoCodes)
            {
                var promo = _promoRepo.Get(pc);
                if(promo != null)
                {
                    promoCollection.Add(promo);
                }
            }
            // process promo code and get total price
            PromoModels.PromofiedCart promofiedCart = _promoEngine.Process(promoCollection, cart);

            return promofiedCart;
        }
    }
}
