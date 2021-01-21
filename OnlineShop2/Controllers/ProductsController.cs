using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Security.Principal;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include("Categories").Include("User");
            ViewBag.Products = products;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            ViewBag.isAdmin = false;
            ViewBag.isProductOwner = false;
            ViewBag.UserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            if (product.UserId == User.Identity.GetUserId())
            {
                ViewBag.isproductOwner = true;
            }
            ViewBag.Product = product;

            return View(product);
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            Product product = new Product();
            product.Cat = getAllCategories();
            product.UserId = User.Identity.GetUserId();
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Product product)
        {
            product.Categories = new Collection<Category>();
            product.Date = DateTime.Now;

            product.UserId = User.Identity.GetUserId();

            try
            {
                if (ModelState.IsValid)
                {

                    foreach(var selectedCategory in product.SelectedCategories)
                    {
                        Category dbCat = db.Categories.Find(selectedCategory);
                        product.Categories.Add(dbCat);
                    }

                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata";

                    return RedirectToAction("Index");
                }
                else
                {
                    product.Cat = getAllCategories();
                    return View(product);
                }

            }
            catch (Exception e)
            {
                product.Cat = getAllCategories();
                return View(product);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            product.UserId = User.Identity.GetUserId();
            product.Cat = getAllCategories();
            List<int> currentSelection = new List<int>();
            foreach(var cat in product.Categories)
            {
                currentSelection.Add(cat.CategoryId);
            }
            product.SelectedCategories = currentSelection.ToArray();
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Product requestProduct)
        {
            requestProduct.Cat = getAllCategories();
            try
            {
                if(ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);
                    if (TryUpdateModel(product))
                    {
                        foreach (Category currentCat in product.Categories.ToList())
                        {
                            product.Categories.Remove(currentCat);
                        }
                        foreach (var selectedCat in requestProduct.SelectedCategories)
                        {
                            Category dbCat = db.Categories.Find(selectedCat);
                            product.Categories.Add(dbCat);
                        }
                        product.Name = requestProduct.Name;
                        product.Description = requestProduct.Description;
                        product.Price = requestProduct.Price;
                        product.Date = DateTime.Now;
                        db.SaveChanges();
                        TempData["message"] = "productarea a fost editata cu succes";
                        return RedirectToAction("Index");

                    }
                    return RedirectToAction("Index");
                }
                return View(requestProduct);
            }
            catch (Exception e)
            {
                return View(requestProduct);
            }
        }


        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult AddToCart(FormCollection formData)
        {
            int id = Int32.Parse(formData.Get("ProductId"));
            Product product = db.Products.Find(id);
            string userid = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userid);
            product.Buyers.Add(user);
            db.SaveChanges();
            return Redirect("/Carts/Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> getAllCategories()
        {
            

            var selectList = new List<SelectListItem>();
            var cats = from cat in db.Categories select cat;
            foreach (var cat in cats)
            {
                var listItem = new SelectListItem();
                listItem.Value = cat.CategoryId.ToString();
                listItem.Text = cat.Name.ToString();
                selectList.Add(listItem);
            }
            return selectList;
        }

    }
}