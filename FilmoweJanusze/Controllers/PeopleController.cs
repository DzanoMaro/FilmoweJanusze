using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using FilmoweJanusze.DAL;
using FilmoweJanusze.Models;
using FilmoweJanusze.ViewModels;
using Microsoft.AspNet.Identity;

namespace FilmoweJanusze.Controllers
{
    public class PeopleController : ExtendedController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: People
        public ActionResult Index(string sortParam, string sortOrder, string proffesion, int? pageSize, int? page)
        {
            IQueryable<People> peoples = db.Peoples.OrderBy(p => p.LastName);

            ViewBag.SortParam = new SelectList(new[] { "Nazwisko", "Imię", "Data urodzenia" }, sortParam);
            ViewBag.SortOrder = new SelectList(new[] { "Rosnąco", "Malejąco" }, sortOrder);
            ViewBag.Proffesion = new SelectList(new[] { "Aktor", "Reżyser", "Scenarzysta"}, proffesion);
            ViewBag.PageSize = new SelectList(new[] { 4, 8, 12, 20 }, pageSize);

            int pagesize = (pageSize ?? 8);
            int pageNumber = (page ?? 1);
            
            switch(proffesion)
            {
                case "Aktor":
                    peoples = peoples.Where(p => p.Proffesion.Actor == true);
                    break;
                case "Reżyser":
                    peoples = peoples.Where(p => p.Proffesion.Director == true);
                    break;
                case "Scenarzysta":
                    peoples = peoples.Where(p => p.Proffesion.Scenario == true);
                    break;
                default:
                    break;
            }

            switch(sortParam)
            {
                case "Nazwisko":
                    if (sortOrder == "Malejąco")
                        peoples = peoples.OrderByDescending(p => p.LastName);
                    else
                        peoples = peoples.OrderBy(p => p.LastName);
                    break;
                case "Imię":
                    if (sortOrder == "Malejąco")
                        peoples = peoples.OrderByDescending(p => p.FirstName);
                    else
                        peoples = peoples.OrderBy(p => p.FirstName);
                    break;
                case "Data urodzenia":
                    if (sortOrder == "Malejąco")
                        peoples = peoples.OrderByDescending(p => p.Birthdate);
                    else
                        peoples = peoples.OrderBy(p => p.Birthdate);
                    break;
                default:
                    peoples = peoples.OrderBy(p => p.LastName);
                    break;
            }

            if (Request.IsAjaxRequest())
                return PartialView("_PeopleList", peoples.ToPagedList(pageNumber, pagesize));

            return View(peoples.ToPagedList(pageNumber, pagesize));
        }
        
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            People people = db.Peoples.Include(p=>p.PeopleInfo).Include(p => p.Proffesion).Include(p=>p.UserRates).Include("Roles.Movie").FirstOrDefault(p => p.PeopleID == id);

            if (people == null)
            {
                return HttpNotFound();
            }

            //pobranie roli filmowych
            //movieandCast.Cast = db.ActorRoles.Where(a => a.PeopleID == movieandCast.People.PeopleID).OrderByDescending(a=>a.Movie.ReleaseDate).ToList();
            //pobranie wyreżyserowanych filmów
            //movieandCast.DirectedMovies = db.Movies.Where(m => m.DirectorID == movieandCast.People.PeopleID).OrderByDescending(m => m.ReleaseDate).ToList();
            //pobranie przykładowych zdjęć
            people.Photos = db.Photos.Where(p => p.PeopleID == id).Take(6).ToList();
            //pobranie ocen użytkowników
            //movieandCast.UserRates = db.UserRates.Where(p => p.PeopleID == id).ToList();
            people.DirectedMovies = db.Movies.Where(m => m.MovieInfo.DirectorID == id).ToList();

            //liczba zdjęć w galerii
            ViewBag.PhotoCount = db.Photos.Where(p => p.PeopleID == id).Count();
            //id usera
            ViewBag.UserID = User.Identity.GetUserId();

            //pobranie oceny zalogowanego usera
            if (people.UserRates.Count > 0)
            {
                ViewBag.Rate = Math.Round(people.UserRates.Average(ur => ur.Rate),2);
                ViewBag.Controller = people.Controller;
                /*
                ViewBag.MovieRate = null;
                ViewBag.PeopleRate = movieandCast.UserRates.Average(ur => ur.Rate);
                if (ViewBag.UserID != null)
                {
                    movieandCast.LoggedInURate = movieandCast.UserRates.FirstOrDefault(ur => ur.UserID == ViewBag.UserID);
                }
                */
            }

            //jeśli nie oceniono jeszcze
            /*
            if (movieandCast.LoggedInURate == null)
            {
                movieandCast.LoggedInURate = new UserRate();
                movieandCast.LoggedInURate.MovieID = null;
                movieandCast.LoggedInURate.Movie = null;
                movieandCast.LoggedInURate.PeopleID = (int)id;
                movieandCast.LoggedInURate.People = movieandCast.People;
            }
            */

            return View(people);
    }
   
        // GET: People/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {

            PeopleFormView peopleFormView = new PeopleFormView();
            peopleFormView.PhotoURL = String.Empty;
            peopleFormView.Birthdate = DateTime.Today;

            return View(peopleFormView);
        }

        // POST: People/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Birthdate,Birthplace,Height,Biography,Proffesion,PhotoURL")] PeopleFormView peopleFormView, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image)
        {
            //CheckBirthday(people.Birthdate);

            if (ModelState.IsValid)
            {

                People people = new People
                {
                    FirstName = peopleFormView.FirstName,
                    LastName = peopleFormView.LastName,
                    Proffesion = peopleFormView.Proffesion,
                    Birthdate = peopleFormView.Birthdate,
                };

                people.PeopleInfo = new PeopleInfo
                {
                    Biography = peopleFormView.Biography,
                    Birthplace = peopleFormView.Birthplace,
                    Height = peopleFormView.Height,
                };


                if (!String.IsNullOrEmpty(RadioPhotoBtn))
                {
                    switch (RadioPhotoBtn)
                    {
                        case "FromFile":
                            if (image != null)
                                people.PhotoURL = SaveNewFile("PeopleFaces", people.FullName, image);
                            else
                                people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                            break;
                        case "FromURL":
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                DeleteOldFile(people.PhotoURL);
                                people.PhotoURL = UrlPath;
                            }
                            break;
                        case "None":
                            DeleteOldFile(people.PhotoURL);
                            people.PhotoURL = NoContentPhoto;
                            break;
                        default:
                            people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                            break;
                    }
                }
                else
                {
                    people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                }
                

                db.Peoples.Add(people);
                db.SaveChanges();
                TempData["Success"] = "Poprawnie dodano postać filmową.";
                return RedirectToAction("Index");
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(peopleFormView);
        }

        // GET: People/Edit/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            People people = db.Peoples.Include(p => p.PeopleInfo).Include(p => p.Proffesion).Where(p => p.PeopleID == id).Single();
            if (people == null)
            {
                return HttpNotFound();
            }

            PeopleFormView peopleFormView = new PeopleFormView
            {
                FirstName = people.FirstName,
                LastName = people.LastName,
                Proffesion = people.Proffesion,
                Birthdate = people.Birthdate,

                Biography = people.PeopleInfo.Biography,
                Birthplace = people.PeopleInfo.Birthplace,
                Height = people.PeopleInfo.Height,
            };

            ViewBag.Name = people.FullName;
            ViewBag.PeopleID = id;
            return View(peopleFormView);
        }

        // POST: People/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image)
        {
            //[Bind(Include = "PeopleID,FirstName,LastName,Birthdate,Birthplace,Height,Biography,FacePhoto,FaceMimeType")] People people

            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var people = db.Peoples.Include(p=>p.PeopleInfo).Include(p => p.Proffesion).Where(p => p.PeopleID == id).Single();

            if (people == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = people.FullName;
            ViewBag.PeopleID = id;

            PeopleFormView peopleFormView = new PeopleFormView
            {
                FirstName = people.FirstName,
                LastName = people.LastName,
                Proffesion = people.Proffesion,
                Birthdate = people.Birthdate,

                Biography = people.PeopleInfo.Biography,
                Birthplace = people.PeopleInfo.Birthplace,
                Height = people.PeopleInfo.Height,
            };

            if (TryUpdateModel(people, "", new string[] {"FirstName", "LastName", "Birthdate", "Proffesion", "PhotoURL" }))
            {
                if (TryUpdateModel(people.PeopleInfo, "", new string[] { "Birthplace", "Height", "Biography"}))
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            if (!String.IsNullOrEmpty(RadioPhotoBtn))
                            {
                                switch (RadioPhotoBtn)
                                {
                                    case "FromFile":
                                        if (image != null)
                                            people.PhotoURL = SaveNewFile("PeopleFaces", people.FullName, image);
                                        else
                                            people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                                        break;
                                    case "FromURL":
                                        if (!String.IsNullOrEmpty(UrlPath))
                                        {
                                            DeleteOldFile(people.PhotoURL);
                                            people.PhotoURL = UrlPath;
                                        }
                                        break;
                                    case "None":
                                        DeleteOldFile(people.PhotoURL);
                                        people.PhotoURL = NoContentPhoto;
                                        break;
                                    default:
                                        people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                                        break;
                                }
                            }
                            else
                            {
                                people.PhotoURL = IfEmptySetEmptyPhoto(people.PhotoURL);
                            }

                            /*
                            if (people.Proffesion.Actor == false && people.Proffesion.Director == false && people.Proffesion.Scenario == false)
                            {
                                people.Proffesion = null;       //usuwa rekord w DB
                            }
                            */
                            db.Entry(people).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["Success"] = "Poprawnie zmieniono informacje o człowieku.";
                            return RedirectToAction("Details", "People", new { id = people.PeopleID });
                        }
                        ViewData["Error"] = "Nie można zapisać, popraw błędy!";
                        return View(peopleFormView);
                    }   
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(peopleFormView);
        }

        // GET: People/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            People people = db.Peoples.Find(id);
            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            People people = db.Peoples.Include(p => p.PeopleInfo).Include(p => p.Proffesion).FirstOrDefault(p => p.PeopleID == id);
            db.Peoples.Remove(people);

            IQueryable actorroles = db.ActorRoles.Where(a => a.PeopleID == id);
            foreach (ActorRole a in actorroles)
                db.ActorRoles.Remove(a);

            db.SaveChanges();
            TempData["Success"] = "Poprawnie usunięto postać filmową.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // ---------------WŁASNE FUNKCJE---------------

        public FileContentResult GetImage(int peopleId)
        {
            /* TODO
            People people = db.Peoples.FirstOrDefault(p => p.PeopleID == peopleId);
            if (people != null)
            {
                return File(people.FacePhoto, people.FaceMimeType);
            }
            else
            {
                
            }
            */
            return null;
        }

        /* ZASTAPIONE ATRYBUTEM
        public void CheckBirthday(DateTime dateTime)
        {
            bool checkOK = false;

            if (dateTime.Year < DateTime.Now.Year)
            {
                checkOK = true;
            }
            else if (dateTime.Year == DateTime.Now.Year)
            {
                if (dateTime.Month < DateTime.Now.Month)
                {
                    checkOK = true;
                }
                else if (dateTime.Month == DateTime.Now.Month)
                {
                    if (dateTime.Day < DateTime.Now.Day)
                    {
                        checkOK = true;
                    }
                }
            }

            if (!checkOK)
                ModelState.AddModelError("Birthdate", "Data urodzenia nie może być z przyszłości");
        }
        */
        }
}
