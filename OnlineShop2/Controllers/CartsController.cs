using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carts
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            ViewBag.User = user;
            List<Product> products = new List<Product>();
            foreach(var product in db.Products)
            {
                foreach(var us in product.Buyers)
                {
                    if(us.Id == id)
                    {
                        products.Add(product);
                        
                    }
                }
            }
            if(products.Count!=0)
            {
                ViewBag.Products = products.ToList();
                ViewBag.ProductsCount = products.Count();
            }
            else
            {
                ViewBag.ProductsCount = 0;
            }
            
            
            return View();
        }
    }
}