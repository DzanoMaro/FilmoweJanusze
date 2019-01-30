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
        public ActionResult Create([Bind(Include = "UserRateID,MovieID,PeopleID,UserID,Rate,Comment")]UserRate userRate)
        {
            if(userRate.MovieID != null && userRate.PeopleID != null)
                ModelState.AddModelError("", "Nie można przypisać oceny jednocześnie do filmu i aktora");
            if (userRate.MovieID == null && userRate.PeopleID == null)
                ModelState.AddModelError("", "Nie można przypisać oceny ani do filmu ani do aktora");

            if (ModelState.IsValid)
            {
                db.UserRates.Add(userRate);
                db.SaveChanges();
                TempData["Success"] = "Twoja ocena została zapisana.";
                if(userRate.MovieID!=null)
                    return RedirectToAction("Details","Movies", new { id= userRate.MovieID});
                if (userRate.PeopleID != null)
                    return RedirectToAction("Details", "People", new { id = userRate.PeopleID });
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
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
            if (TryUpdateModel(userRate, "", new string[] { "UserRateID", "MovieID", "PeopleID", "UserID", "Rate", "Comment" }))
            {
                try
                {
                    if (userRate.MovieID != null && userRate.PeopleID != null)
                        ModelState.AddModelError("", "Nie można przypisać oceny jednocześnie do filmu i aktora");
                    if (userRate.MovieID == null && userRate.PeopleID == null)
                        ModelState.AddModelError("", "Nie można przypisać oceny ani do filmu ani do aktora");

                    if (ModelState.IsValid)
                    {
                        //userRate.User = db.Users.Find(userRate.User.Id);
                        // TODO: Add insert logic here

                        db.Entry(userRate).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Success"] = "Twoja ocena została zmieniona.";
                        if (userRate.MovieID != null)
                            return RedirectToAction("Details", "Movies", new { id = userRate.MovieID });
                        if (userRate.PeopleID != null)
                            return RedirectToAction("Details", "People", new { id = userRate.PeopleID });
                    }
                    ViewData["Error"] = "Nie można zapisać, popraw błędy!";
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

                    int? movieID=null, peopleID=null;

                    if (userRate.MovieID != null)
                        movieID = userRate.MovieID.Value;
                    if (userRate.PeopleID != null)
                        peopleID = userRate.PeopleID.Value;

                    db.UserRates.Remove(userRate);
                    db.SaveChanges();
                    TempData["Success"] = "Twoja ocena została usunięta.";

                    if (movieID != null)
                        return RedirectToAction("Details", "Movies", new { id = movieID });
                    if (peopleID != null)
                        return RedirectToAction("Details", "People", new { id = peopleID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
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
