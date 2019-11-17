using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using LifeMall.Models;
using System.Web.Mvc;

namespace LifeMall.Controllers
{
    public class CreateController : AccountController
    {
        // GET: Create
        public ActionResult CreateCategory(string categoryName,string parentID)
        {
            using(var db = new LifeMallDBContext()) 
            {
                var newCategory = new Category()
                {
                    CategoryName = categoryName,
                    ParentID = int.Parse(parentID)

                };
                db.Category.Add(newCategory);
                db.SaveChanges();


            }


            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCategory(string categoryName,string categoryID)
        {
            using (var db = new LifeMallDBContext())
            {
                int cateID = int.Parse(categoryID);
                var categoryQuery = db.Category.Where(m => m.CategoryID == cateID).FirstOrDefault();
                if(categoryQuery!=null)
                {
                    categoryQuery.CategoryName = categoryName;
                    db.SaveChanges();
                }
                

            }


            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveCategory(string categoryID)
        {
            using (var db = new LifeMallDBContext())
            {
                int cateID = int.Parse(categoryID);
                var categoryQuery = db.Category.Where(m => m.CategoryID == cateID).FirstOrDefault();
                if (categoryQuery != null)
                {
                    db.Category.Remove(categoryQuery);
                    db.SaveChanges();
                }


            }



            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateProduct(string icon1,string icon2,string categoryID, string description,string price,string productName)
        {
            //string[] textArray = icon1.Split(',');
            //var byt = textArray.Select(byte.Parse).ToArray();
            //Image image = null;
            //Bitmap bitmap = null;
            //using (StreamWriter sw = new StreamWriter(@"C:\abc.txt"))   //小寫TXT     
            //{
            //    // Add some text to the file.
            //    sw.Write("This is the ");
            //    sw.WriteLine("header for the file.");
            //    sw.WriteLine("-------------------");
            //    // Arbitrary objects can also be written to the file.
            //    sw.Write("The date is: ");
            //    sw.WriteLine(DateTime.Now);
            //}
            //using (MemoryStream memoryStream = new MemoryStream(byt))
            //{
            //memoryStream.Position = 0;
            //image = Image.FromStream(memoryStream);
            //bitmap = new Bitmap(image);
            // image.Save("../ProductImage/"+sCode+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //ByteStrToImage
            // GetRandomText

            var path = Server.MapPath("~/ProductImage/");
            string randomText1 = ByteStrToImage(icon1, path);
            string randomText2 = ByteStrToImage(icon2, path);

            using (var db = new LifeMallDBContext())
                {
                    var newProduct = new Product()
                    {
                        CategoryID=int.Parse(categoryID),
                        Description=description,
                        ImageUrl1= "ProductImage/"+ randomText1,
                        ImageUrl2= "ProductImage/" + randomText2,
                        Price=int.Parse(price),
                        ProductName=productName
                        
                    };
                    db.Product.Add(newProduct);
                    db.SaveChanges();
                
                }




            

            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
         
        }
        public ActionResult EditProduct(string icon1, string icon2,string productID, string description, string price, string productName)
        {
            var path = Server.MapPath("~/ProductImage/");
            string randomText1 = ByteStrToImage(icon1, path);
            string randomText2 = ByteStrToImage(icon2, path);
           

            using (var db = new LifeMallDBContext())
            {
                int productIDNum = int.Parse(productID);
                var productQuery = db.Product.Where(m => m.ProductID == productIDNum).FirstOrDefault();
                if(productQuery!=null)
                {

                    productQuery.Price = int.Parse(price);
                    productQuery.ImageUrl1 = "ProductImage/" + randomText1;
                    productQuery.ImageUrl2 = "ProductImage/" + randomText2;
                    productQuery.ProductName = productName;
                    productQuery.Description = description;
                    db.SaveChanges();
                }
                
                

            }


            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveProduct(string productID)
        {
            using (var db = new LifeMallDBContext())
            {
                int productIDNum = int.Parse(productID);
                    var productQuery = db.Product.Where(m => m.ProductID == productIDNum).FirstOrDefault();
                if(productQuery!=null)
                {
                    db.Product.Remove(productQuery);
                    db.SaveChanges();
                }
            }


                var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }


        public class JsonMessage
        {
            string _data = "";
            public string data
            {
                get
                {
                    return _data;
                }

                set
                {
                    _data = value;
                }
            }
        }

        public string GetRandomText()
        {
            char[] CharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string sCode = "";
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                sCode += CharArray[random.Next(CharArray.Length)];
            }

            return sCode;
        }

        public string ByteStrToImage(string iconString,string path)
        {
            string[] textArray = iconString.Split(',');
            var byt = textArray.Select(byte.Parse).ToArray();
            Image image = null;
            Bitmap bitmap = null;

            using (MemoryStream memoryStream = new MemoryStream(byt))
            {
                memoryStream.Position = 0;
                image = Image.FromStream(memoryStream);
                bitmap = new Bitmap(image);
                string randomText = GetRandomText();
                image.Save(path + randomText + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                return randomText;
            }
            
            


        }
        [HttpPost]
        public ActionResult AddCart(string quantity, string productID)
        {
            int quan = int.Parse(quantity);
            int proID = int.Parse(productID);
            if (IsLogin())
            {
            User user = GetUser();
                using (var db = new LifeMallDBContext())
                {
                    var cartQuery = db.Cart.Where(m => m.MemberID == user.MemberID && m.ProductID == proID).FirstOrDefault();
                    if (cartQuery == null)
                    {
                    var newCart = new Cart()
                        {
                            MemberID=user.MemberID,
                            ProductID=proID,
                            Quantity = quan


                        };
                        db.Cart.Add(newCart);
                    }
                    else
                    {
                        cartQuery.Quantity += quan;
                    }
                    db.SaveChanges();
                }
            }

            var j = "{\"resp\":0}";
            return Json(j, JsonRequestBehavior.AllowGet);
        }


    }

}