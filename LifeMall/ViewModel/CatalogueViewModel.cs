using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifeMall.ViewModel
{
    public class CatalogueViewModel
    {
        
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<ProductInCategory> ProductsInCategory = new List<ProductInCategory>();
        
    }

    public class ProductInCategory
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int ProductID { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }


    }


}