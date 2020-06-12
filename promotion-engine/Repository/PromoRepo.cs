using Microsoft.CodeAnalysis.CSharp.Syntax;
using promotion_engine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace promotion_engine.Repository
{
    public class PromoRepo : IPromoRepo
    {
        PromoModels.Promo IPromoRepo.Get(string id)
        {            
            var defaultPromos = GetDefaultPromos().ToList();
            var promo = defaultPromos.Where(p => p.Id == id).FirstOrDefault();
            return promo;
        }

        IEnumerable<PromoModels.Promo> IPromoRepo.GetAll()
        {
            return GetDefaultPromos();
        }

        private IEnumerable<PromoModels.Promo> GetDefaultPromos()
        {
            var promo1 = new PromoModels.Promo
            {
                Id = "promo1",
                Description = "3 of A's for 130",
                Buys = new List<PromoModels.Buy>
                {
                    new PromoModels.Buy()
                    {
                        Category = "A",
                        Count = 3
                    }
                },
                Gets = new List<PromoModels.Get>
                {
                    new PromoModels.Get()
                    {
                        Category = "A",
                        All = true,
                        Off = new PromoModels.Off()
                        {
                            Discount = new PromoModels.Discount() { Percentage = 0 },
                            Fixed = new PromoModels.Fixed() { Price = 130 }
                        }
                    }
                }
            };

            var promo2 = new PromoModels.Promo
            {
                Id = "promo1",
                Description = "B",
                Buys = new List<PromoModels.Buy>
                {
                    new PromoModels.Buy()
                    {
                        Category = "B",
                        Count = 2
                    }
                },
                Gets = new List<PromoModels.Get>
                {
                    new PromoModels.Get()
                    {
                        Category = "",
                        All = true,
                        Off = new PromoModels.Off()
                        {
                            Discount = new PromoModels.Discount() { Percentage = 0 },
                            Fixed = new PromoModels.Fixed() { Price = 45 }
                        }
                    }
                }
            };

            var promo3 = new PromoModels.Promo
            {
                Id = "promo1",
                Description = "",
                Buys = new List<PromoModels.Buy>
                {
                    new PromoModels.Buy()
                    {
                        Category = "C",
                        Count = 1
                    },
                    new PromoModels.Buy()
                    {
                        Category = "D",
                        Count = 1
                    }
                },
                Gets = new List<PromoModels.Get>
                {
                    new PromoModels.Get()
                    {
                        Category = "",
                        All = true,
                        Off = new PromoModels.Off()
                        {
                            Discount = new PromoModels.Discount() { Percentage = 0 },
                            Fixed = new PromoModels.Fixed() { Price = 30 }
                        }
                    }
                }
            };

            return new List<PromoModels.Promo> { promo1, promo2, promo3 };
        }
    }
}