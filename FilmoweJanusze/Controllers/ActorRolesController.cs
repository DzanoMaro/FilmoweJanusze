using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmoweJanusze.DAL;
using FilmoweJanusze.Models;
using FilmoweJanusze.ViewModels;

namespace FilmoweJanusze.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class ActorRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /* DO USUNIĘCIA
        // GET: ActorRoles
        public ActionResult Index(int movieID)
        {
            if(movieID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MovieandCast movieandCast = new MovieandCast();
            movieandCast.Cast = db.ActorRoles.Include(a => a.Movie).Include(a => a.People).Where(a => a.MovieID == movieID).ToList();
            movieandCast.Movie = db.Movies.Where(a => a.MovieID == movieID).First();

            return View(movieandCast);
        }
        */

        // GET: ActorRoles/Create
        public ActionResult Create(int? movieID, int? peopleID)
        {
            if ((movieID == null && peopleID == null) || (movieID != null && peopleID != null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (movieID != null)
            {
                Movie movie = db.Movies.Find(movieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = movieID;
                ViewBag.PeopleID = new SelectList(db.Peoples.Where(p => p.Proffesion.Actor == true), "PeopleID", "FullName");
                ViewBag.Name = movie.TitleYear;
            }
            else 
            if (peopleID != null)
            {
                People people = db.Peoples.Find(peopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "TitleYear");
                ViewBag.PeopleID = peopleID;
                ViewBag.Name = people.FullName;
            }
            return View();
        }

        // POST: ActorRoles/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActorRoleID,PeopleID,MovieID,RoleName,Dubbing")] ActorRole actorRole, bool? ismovie, bool? ispeople)
        {
            if ((ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ismovie != null)
            {
                Movie movie = db.Movies.Find(actorRole.MovieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = actorRole.MovieID;
                ViewBag.PeopleID = new SelectList(db.Peoples.Where(p => p.Proffesion.Actor == true), "PeopleID", "FullName");
                ViewBag.Name = actorRole.Movie.TitleYear;
            }
            else
            if (ispeople != null)
            {
                People people = db.Peoples.Find(actorRole.PeopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "TitleYear");
                ViewBag.PeopleID = actorRole.PeopleID;
                ViewBag.Name = actorRole.People.FullName;
            }

            if (ModelState.IsValid)
            {
                db.ActorRoles.Add(actorRole);
                db.SaveChanges();
                if (ismovie == true)
                {
                    TempData["Success"] = "Członek obsady został dodany.";
                    return RedirectToAction("Details", "Movies", new { id = actorRole.MovieID });
                }
                else if (ispeople == true)
                {
                    TempData["Success"] = "Rola została dodana";
                    return RedirectToAction("Details", "People", new { id = actorRole.PeopleID });
                }
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View();
        }

        // GET: ActorRoles/Edit/5
        public ActionResult Edit(int? id, bool? ismovie, bool? ispeople)
        {
            if ((id == null) || (ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ActorRole actorRole = db.ActorRoles.Find(id);
            if (actorRole == null)
            {
                return HttpNotFound();
            }

            if (ismovie != null)
            {
                ViewBag.MovieID = actorRole.MovieID;
                ViewBag.PeopleID = new SelectList(db.Peoples.Where(p => p.Proffesion.Actor == true), "PeopleID", "FullName", actorRole.PeopleID);
                ViewBag.Name = actorRole.Movie.TitleYear;
            }
            else
            if (ispeople != null)
            {
                ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "TitleYear", actorRole.MovieID);
                ViewBag.PeopleID = actorRole.PeopleID;
                ViewBag.Name = actorRole.People.FullName;
            }

            return View(actorRole);
        }

        // POST: ActorRoles/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActorRoleID,PeopleID,MovieID,RoleName,Dubbing")] ActorRole actorRole, bool? ismovie, bool? ispeople)
        {
            if ((ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ismovie != null)
            {
                Movie movie = db.Movies.Find(actorRole.MovieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                actorRole.Movie = movie;
                ViewBag.MovieID = actorRole.MovieID;
                ViewBag.PeopleID = new SelectList(db.Peoples.Where(p => p.Proffesion.Actor == true), "PeopleID", "FullName", actorRole.PeopleID);
                ViewBag.Name = actorRole.Movie.TitleYear;
            }
            else
            if (ispeople != null)
            {
                People people = db.Peoples.Find(actorRole.PeopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                actorRole.People = people;
                ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "TitleYear", actorRole.MovieID);
                ViewBag.PeopleID = actorRole.PeopleID;
                ViewBag.Name = actorRole.People.FullName;
            }

            if (ModelState.IsValid)
            {
                db.Entry(actorRole).State = EntityState.Modified;
                db.SaveChanges();
                if (ismovie == true)
                {
                    TempData["Success"] = "Zmieniono informacje o członku obsady.";
                    return RedirectToAction("Details", "Movies", new { id = actorRole.MovieID });
                }
                else if (ispeople == true)
                {
                    TempData["Success"] = "Zmieniono informacje o roli.";
                    return RedirectToAction("Details", "People", new { id = actorRole.PeopleID });
                }
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View();
        }

        // GET: ActorRoles/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? ismovie, bool? ispeople)
        {
            if ((id == null) || (ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ActorRole actorRole = db.ActorRoles.Find(id);
            if (actorRole == null)
            {
                return HttpNotFound();
            }

            if (ismovie != null)
            {
                ViewBag.MovieID = actorRole.MovieID;
                ViewBag.PeopleID = new SelectList(db.Peoples.Where(p => p.Proffesion.Actor == true), "PeopleID", "FullName");
                ViewBag.Name = actorRole.Movie.TitleYear;
            }
            else
            if (ispeople != null)
            {
                ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "TitleYear");
                ViewBag.PeopleID = actorRole.PeopleID;
                ViewBag.Name = actorRole.People.FullName;
            }

            return View(actorRole);
        }

        // POST: ActorRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, bool? ismovie, bool? ispeople)
        {
            if ((id == null) || (ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ActorRole actorRole = db.ActorRoles.Find(id);
            if (actorRole == null)
            {
                return HttpNotFound();
            }

            int redirectid = 0;
            if (ismovie == true)
            {
                redirectid = actorRole.MovieID;
            }
            else if (ispeople == true)
            {
                redirectid = actorRole.PeopleID;
            }

            db.ActorRoles.Remove(actorRole);
            db.SaveChanges();

            if (ismovie == true)
            {
                TempData["Success"] = "Członek obsady został usunięty.";
                return RedirectToAction("Details", "Movies", new { id = redirectid });
            }
            else if (ispeople == true)
            {
                TempData["Success"] = "Rola została usunięta.";
                return RedirectToAction("Details", "People", new { id = redirectid });
            }
            TempData["Success"] = "Usunięto.";
            return RedirectToAction("Index", "Home");
        }

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
