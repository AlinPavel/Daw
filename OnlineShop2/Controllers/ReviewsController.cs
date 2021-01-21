using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ReviewsController : Controller
    {
        // GET: Reviews
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Review review)
        {
            review.Date = DateTime.Now;
            review.UserId = User.Identity.GetUserId();
            try
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return Redirect("/Products/Show/" + review.ProductId);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return Redirect("/Products/Show/" + review.ProductId);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.UserId == User.Identity.GetUserId() || review.Product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
                return Redirect("/Products/Show/" + review.ProductId);
            }
            else
            {
                return Redirect("/Products/Show/" + review.ProductId);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            ViewBag.Review = review;
            if (review.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return Redirect("/Products/Show/" + review.ProductId);

            }
        }


        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Review reqReview)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Review comm = db.Reviews.Find(id);
                    if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(comm))
                        {
                            comm.Content = reqReview.Content;
                            db.SaveChanges();
                        }
                        return Redirect("/Products/Show/" + comm.ProductId);
                    }
                    else
                    {
                        return Redirect("/Products/Show/" + comm.ProductId);
                    }
                }
                else
                {
                    return View(reqReview);
                }
            }
            catch (Exception e)
            {
                return View(reqReview);
            }
        }
    }
}