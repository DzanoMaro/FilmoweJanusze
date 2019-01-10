using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.Controllers
{
    public class UserRateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserRate/Create
        /*
        public ActionResult Create()
        {
            return View();
        }
        */
        // POST: UserRate/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "UserRateID,MovieID,User,Rate,Comment")]UserRate userRate)
        {
            if (ModelState.IsValid)
            {
                userRate.User = db.Users.Find(userRate.User.Id);
                // TODO: Add insert logic here
                db.UserRates.Add(userRate);
                db.SaveChanges();

                return RedirectToAction("Details","Movies", new { id= userRate.MovieID});
            }

            return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
        }
        /*
        // GET: UserRate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        */
        // POST: UserRate/Edit/5
        [HttpPost]
        public ActionResult Edit(UserRate userRate)
        {
            if (TryUpdateModel(userRate, "", new string[] { "UserRateID", "MovieID", "User", "Rate", "Comment" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        //userRate.User = db.Users.Find(userRate.User.Id);
                        // TODO: Add insert logic here

                        db.Entry(userRate).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
                    }

                    return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            if (userRate.Rate == 0)
            {
                //BRAK OCENY => USUN OCENE
                try
                {
                    userRate = db.UserRates.Where(ur => ur.UserRateID == userRate.UserRateID).Single();
                    db.UserRates.Remove(userRate);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
        }
        /*
        // GET: UserRate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        */
        // POST: UserRate/Delete/5
        /*
        [HttpPost]
        public ActionResult Delete(int id)
        {
            UserRate userRate = db.UserRates.Where(ur => ur.UserRateID == id).Single();


            return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
