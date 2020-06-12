using promotion_engine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Repository
{
    public interface ICartRepo
    {
        CartModels.Cart GetCart(string id);

        string AddCart(CartModels.Cart cart);

        bool DeleteCart(string id);

        PromoModels.PromofiedCart ApplyPromo(string id);
    }
}
