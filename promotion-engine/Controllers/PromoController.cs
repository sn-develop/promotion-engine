using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using promotion_engine.Models;
using promotion_engine.Repository;

namespace promotion_engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoController : ControllerBase
    {
        private readonly IPromoRepo _promo;

        public PromoController(IPromoRepo promo)
        {
            _promo = promo;
        }

        // GET: api/Promos
        [HttpGet(Name = "GetPromos")]
        public List<PromoModels.Promo> Get()
        {
            return _promo.GetAll().ToList();
        }

        // GET: api/Promo/5
        [HttpGet("{id}", Name = "GetPromo")]
        public PromoModels.Promo Get(string id)
        {
            return _promo.Get(id);
        }        
    }
}
