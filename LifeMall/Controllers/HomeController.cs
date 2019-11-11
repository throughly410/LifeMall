using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeMall.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult Catalogue()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            return View();
        }


        public ActionResult ProductManage()
        {
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