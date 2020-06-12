using Microsoft.VisualStudio.TestTools.UnitTesting;
using promotion_engine.Models;
using promotion_engine.Processor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace promotion_engine_tests
{
    [TestClass]
    public class PromoEngineTests
    {
        IPromoEngine promoEngine = new PromoEngine();

        [TestMethod]
        public void processEnginePromo1()
        {
            // Arrange
            CartModels.Cart cart = new CartModels.Cart
            {
                Id = "1",
                Items = new List<CartModels.Item> {
                    new CartModels.Item
                    {
                        Id = "1",
                        Name = "A",
                        Category = "Product A",
                        Price = 50F
                    },
                    new CartModels.Item
                    {
                        Id = "1",
                        Name = "A",
                        Category = "Product A",
                        Price = 50F
                    },
                    new CartModels.Item
                    {
                        Id = "1",
                        Name = "A",
                        Category = "Product A",
                        Price = 50F
                    }
                }
            };

            List<PromoModels.Promo> promos = new List<PromoModels.Promo>
            {
                new PromoModels.Promo {
                    Id = "promo1",
                    Buys = new List<PromoModels.Buy>
                    {
                        new PromoModels.Buy
                        {
                            Category = "Product A",
                            Count = 2
                        }
                    },
                    Description = "3 of A's for 130",   // same as Buy 2 A's and get third for 30 
                    Gets = new List<PromoModels.Get>
                    {
                        new PromoModels.Get
                        {
                            Category = "Product A",
                            All = true,
                            Count = 0,
                            Off = new PromoModels.Off
                            {
                                Fixed = new PromoModels.Fixed{ Price = 30F }                                
                            }
                        }
                    }
                }
            };


            // Act
            var promoFied = promoEngine.Process(promos, cart);


            // Assert
            Assert.AreEqual(promoFied.TotalPrice, 150F);
            Assert.AreEqual(promoFied.TotalOffPrice, 130F);

        }

        [TestMethod]
        public void processEnginePromo2()
        {
            // Arrange
            CartModels.Cart cart = new CartModels.Cart
            {
                Id = "1",
                Items = new List<CartModels.Item> {
                    new CartModels.Item
                    {
                        //Id = "1",
                        //Name = "A",
                        Category = "Product A",
                        Price = 100F
                    },
                    new CartModels.Item
                    {
                        //Id = "1",
                       //Name = "A",
                        Category = "Product A",
                        Price = 200F
                    },
                    new CartModels.Item
                    {
                        //Id = "1",
                        //Name = "A",
                        Category = "Product A",
                        Price = 130F
                    },
                    new CartModels.Item
                    {
                        //Id = "1",
                        //Name = "A",
                        Category = "Product A",
                        Price = 50F
                    }
                }
            };

            List<PromoModels.Promo> promos = new List<PromoModels.Promo>
            {
                new PromoModels.Promo {
                    Id = "promo1",
                    Buys = new List<PromoModels.Buy>
                    {
                        new PromoModels.Buy
                        {
                            Category = "Product A",
                            Count = 2
                        }
                    },
                    Description = "buy 2 and get 1 free",
                    Gets = new List<PromoModels.Get>
                    {
                        new PromoModels.Get
                        {
                            Category = "Product A",
                            //All = false,
                            Count = 1,
                            Off = new PromoModels.Off
                            {
                                //Fixed = new PromoModels.Fixed{ Price = 0F },
                                Discount = new PromoModels.Discount{ Percentage = 100F}
                            }
                        }
                    }
                }
            };


            // Act
            var promoFied = promoEngine.Process(promos, cart);


            // Assert
            Assert.AreEqual(promoFied.TotalPrice, 480F);
            Assert.AreEqual(promoFied.TotalOffPrice, 350F);
        }
    }
}
