using promotion_engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Processor
{
    public interface IPromoEngine
    {
        PromoModels.PromofiedCart Process(List<PromoModels.Promo> promos, CartModels.Cart cart);
    }
}
