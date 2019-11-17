using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeMall.Models;
using LifeMall.ViewModel;
using Newtonsoft.Json;
namespace LifeMall.Controllers
{
    public class ViewRepositoryController : AccountController
    {
        public ActionResult GetCategory()
        {
            using (var db = new LifeMallDBContext())
            {
                var categoryQuery = db.Category.ToList();

                return Json(JsonConvert.SerializeObject(categoryQuery),JsonRequestBehavior.AllowGet);

            }




                
        }
        public ActionResult GetProduct(int categoryID)
        {
            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Where(m => m.CategoryID == categoryID).ToList();
                return Json(JsonConvert.SerializeObject(productQuery), JsonRequestBehavior.AllowGet);


            }


        }

        public ActionResult GetIndex()
        {
            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Take(8).ToList();
                return Json(JsonConvert.SerializeObject(productQuery), JsonRequestBehavior.AllowGet);


            }


        }
        public ActionResult GetProductDetail(int? productID)
        {

            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Where(m => m.ProductID == productID).FirstOrDefault();
                return Json(JsonConvert.SerializeObject(productQuery), JsonRequestBehavior.AllowGet);


            }


        }

        public ActionResult GetCatalogue()
        {

            using (var db = new LifeMallDBContext())
            {
                var productQuery = db.Product.Join(
                    db.Category,
                    c => c.CategoryID,
                    s => s.CategoryID,
                    (c, s) => new
                    {
                        c.ProductName,
                        c.Price,
                        c.ProductID,
                        c.ImageUrl1,
                        c.ImageUrl2,
                        c.CategoryID,
                        s.CategoryName
                    }).OrderBy(cs=>cs.CategoryID).ToList();
                if (productQuery != null)
                {
                List<CatalogueViewModel> catalogueList = new List<CatalogueViewModel>();
                int currentID = productQuery[0].CategoryID, tempID=-1, index = -1;
                
                foreach (var item in productQuery)
                {
                    currentID = item.CategoryID;
                    if (currentID != tempID)
                    {
                        index++;
                        catalogueList.Add(new CatalogueViewModel());
                        catalogueList[index].CategoryID = item.CategoryID;
                        catalogueList[index].CategoryName = item.CategoryName;
                    }

                    catalogueList[index].ProductsInCategory.Add(new ProductInCategory()
                    {
                        ProductName = item.ProductName,
                        ProductID = item.ProductID,
                        Price = item.Price,
                        ImageUrl1 = item.ImageUrl1,
                        ImageUrl2 = item.ImageUrl2
                    });



                    tempID = currentID;

                }

                return Json(JsonConvert.SerializeObject(catalogueList), JsonRequestBehavior.AllowGet);
                }
                return null;

            }


        }

        public ActionResult GetCart()
        {
            if (IsLogin())
            {
            using (var db = new LifeMallDBContext())
                {
                    User user = GetUser();
                    var cartQuery = db.Cart.Where(m => m.MemberID == user.MemberID).Join
                        (db.Product,
                        c=>c.ProductID,
                        s=>s.ProductID,
                        (c,s)=>new
                        {
                            c.CartID,
                            c.Quantity,
                            s.Price,
                            s.ProductName,
                            s.ImageUrl1,
                            

                        }).ToList();
                    return Json(JsonConvert.SerializeObject(cartQuery), JsonRequestBehavior.AllowGet);

                }

            }

            return null;

        }


    }
}