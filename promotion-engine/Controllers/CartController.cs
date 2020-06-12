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
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        // GET: api/Cart/5
        [HttpGet("{id}", Name = "GetCart")]
        public CartModels.Cart Get(string id)
        {
            return _cartRepo.GetCart(id);
        }

        // POST: api/Cart
        [HttpPost(Name = "AddCart")]
        public string Post([FromBody] CartModels.Cart cart) 
        {
            return _cartRepo.AddCart(cart);
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}", Name = "DeleteCart")]
        public void Delete(string id)
        {
            _cartRepo.DeleteCart(id);
            return;
        }

        // APPLY PROMOS: api/ApplyPromo
        [HttpGet("{id}", Name = "ApplyPromo")]
        public PromoModels.PromofiedCart Apply(string id)
        {
            return _cartRepo.ApplyPromo(id);
        }
    }
}
