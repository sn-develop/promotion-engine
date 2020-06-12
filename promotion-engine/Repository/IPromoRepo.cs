using promotion_engine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Repository
{
    public interface IPromoRepo
    {
        PromoModels.Promo Get(string id);
        IEnumerable<PromoModels.Promo> GetAll(); 
    }
}
