using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeMall.Models;
using LifeMall.ViewModel;
namespace LifeMall.Controllers
{   [Authorize]
    public class HomeController : AccountController
    {
        public ActionResult Index()
        {

            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Take(8).ToList();


                return View(productQuery);
            }
        }
        

        public ActionResult Catalogue()
        {
            return View();
        }
        public ActionResult Product(int?　productID)
        {
            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Where(m => m.ProductID == productID).FirstOrDefault();
                return View(productQuery);

            }



                
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            using (var db = new LifeMallDBContext())
            {
                if (IsLogin())
                {
                    
                    
                        User user = GetUser();
                        var cartQuery = db.Cart.Where(m => m.MemberID == user.MemberID).Join
                            (db.Product,
                            c => c.ProductID,
                            s => s.ProductID,
                            (c, s) => new
                            {
                                c.CartID,
                                c.Quantity,
                                s.Price,
                                s.ProductName,
                                s.ImageUrl1


                            }).ToList();
                    var checkOutViewModel = new CheckOutVIewModel();
                    int total = 0;
                    foreach (var item in cartQuery)
                    {
                        checkOutViewModel.Total += item.Quantity * item.Price;
                    }
                    return View(checkOutViewModel);

                }





                return View();
            }
        }
        [HttpPost]
        public ActionResult CheckOut(string receiver, string address, string zipCode, string phone)
        {
            if (IsLogin())
            {
                var user = GetUser();
                using(var db = new LifeMallDBContext()) 
                {
                    var cartQuery = db.Cart.Where(m => m.MemberID == user.MemberID).Join
                            (db.Product,
                            c => c.ProductID,
                            s => s.ProductID,
                            (c, s) => new
                            {
                                c.ProductID,
                                c.Quantity,
                                s.Price,
                            }).ToList();

                    Guid guid = Guid.NewGuid();

                    var newOrderList = new OrderList()
                    {
                        Receiver = receiver,
                        Address = address,
                        Phone = phone,
                        OrderListID = guid,
                        MemberID = user.MemberID,
                        ZipCode = zipCode,
                        OrderDate = DateTime.Now

                        
                    };
                    db.OrderList.Add(newOrderList);
                    foreach (var item in cartQuery)
                    {
                        var newOrderDetail = new OrderDetail()
                        {
                            OrderListID = guid,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity

                        };
                        db.OrderDetail.Add(newOrderDetail);
                    }

                   // db.Cart.Remove
                    db.Cart.RemoveRange(db.Cart.Where(m => m.MemberID == user.MemberID));
                    db.SaveChanges();
                }


            }





            return RedirectToAction("Index","Home");
        }




        public ActionResult ProductManage(int? categoryID)
        {


            ViewBag.categoryID = categoryID;
            return View();
        }
        public ActionResult CategoryManage()
        {


            return View();
        }

        public ActionResult Test()
        {
            return View();
        }







    }
}