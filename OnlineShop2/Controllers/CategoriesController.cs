using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            var categories = db.Categories;
            ViewBag.Categories = categories;
            ViewBag.isAdmin = false;
            if(User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Category category = new Category();
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "Tag submited";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch
            {
                return View(category);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category reqCat)
        {
            try
            {
                Category cat = db.Categories.Find(id);
                if (TryUpdateModel(cat))
                {
                    cat.Name = reqCat.Name;
                    db.SaveChanges();
                   
                    return RedirectToAction("Index");
                }
                return View(reqCat);
            }
            catch
            {
                return View(reqCat);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa";
            return RedirectToAction("Index");
        }

    }
}