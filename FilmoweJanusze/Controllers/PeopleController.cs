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
            IEnumerable<People> peoples = db.Peoples.Include(p=>p.Proffesion).Include(p => p.UserRates).ToList();

            ViewBag.SortParam = new SelectList(new[] { "Nazwisko", "Imię", "Data urodzenia", "Oceny" }, sortParam);
            ViewBag.SortOrder = new SelectList(SortOrder(), sortOrder);
            ViewBag.Proffesion = new SelectList(Proffesion.GetProffesions(), proffesion);
            ViewBag.PageSize = new SelectList(PageSizes(), pageSize);
            ViewBag.NoContent = "Brak postaci kina spełniających warunek :(";

            int pagesize = (pageSize ?? DefPageSize);
            int pageNumber = (page ?? DefPageNo);

            peoples = SwitchProffesion(peoples, proffesion);
            peoples = SortPeoples(peoples, sortParam, sortOrder);            

            if (Request.IsAjaxRequest())
                return PartialView("_TileList", peoples.ToPagedList(pageNumber, pagesize));

            return View(peoples.ToPagedList(pageNumber, pagesize));
        }
        
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            CheckID(id);
            
            People people = db.Peoples.Include(p=>p.PeopleInfo).Include(p => p.Proffesion).Include(p=>p.UserRates).Include("Roles.Movie").FirstOrDefault(p => p.PeopleID == id);

            if (people == null)
            {
                return HttpNotFound();
            }
            people.Roles = people.Roles.OrderByDescending(r => r.Movie.ReleaseDate).ToList();
            people.Photos = db.Photos.Where(p => p.PeopleID == id).Take(6).ToList();
            people.DirectedMovies = db.Movies.Where(m => m.MovieInfo.DirectorID == id).Select(m=>m.MovieInfo).Include(m=>m.Movie).OrderByDescending(m=>m.Movie.ReleaseDate).ToList();

            //liczba zdjęć w galerii
            ViewBag.PhotoCount = db.Photos.Where(p => p.PeopleID == id).Count();
            //id usera
            ViewBag.UserID = User.Identity.GetUserId();

            //pobranie oceny zalogowanego usera
            if (people.UserRates.Count > 0)
            {
                ViewBag.Rate = Math.Round(people.UserRates.Average(ur => ur.Rate),2);
                ViewBag.RateCount = people.UserRates.Count();
                ViewBag.Controller = people.Controller;
            }

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
                RowVersion = people.PeopleInfo.RowVersion,
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
        public ActionResult Edit(int? id, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image, byte[] rowVersion)
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

            if (TryUpdateModel(people, "", new string[] {"FirstName", "LastName", "Birthdate", "Proffesion", "PhotoURL" }))
            {
                if (TryUpdateModel(people.PeopleInfo, "", new string[] { "Birthplace", "Height", "Biography", "RowVersion"}))
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(people.PeopleInfo).OriginalValues["RowVersion"] = rowVersion;

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
                    }   
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var PeopleInfo_DBEntry = ex.Entries.Single().GetDatabaseValues();

                        if (PeopleInfo_DBEntry == null)
                        {
                            ModelState.AddModelError(string.Empty, "Postać, którą próbujesz zmienić, została już usunięta.");
                        }
                        else
                        {
                            var PeopleInfo_DBValues = (PeopleInfo)PeopleInfo_DBEntry.ToObject();
                            var People_DBValues = (People)db.Entry(people).GetDatabaseValues().ToObject();
                            var Proffesion_DBValues = (Proffesion)db.Entry(people.Proffesion).GetDatabaseValues().ToObject();

                            if (People_DBValues.FirstName != people.FirstName)
                                ModelState.AddModelError("FirstName", "Aktualna wartość: " + People_DBValues.FirstName);
                            if (People_DBValues.LastName != people.LastName)
                                ModelState.AddModelError("LastName", "Aktualna wartość: " + People_DBValues.LastName);
                            if (People_DBValues.Birthdate != people.Birthdate)
                                ModelState.AddModelError("Birthdate", "Aktualna wartość: " + People_DBValues.Birthdate.ToShortDateString());
                            if (People_DBValues.PhotoURL != people.PhotoURL)
                                ModelState.AddModelError(String.Empty, "Zmienione zostało zdjęcie główne");

                            if (!Proffesion_DBValues.EqualTo(people.Proffesion))
                                ModelState.AddModelError(String.Empty, "Zmieniony został zawód postaci");

                            if (PeopleInfo_DBValues.Birthplace != people.PeopleInfo.Birthplace)
                                ModelState.AddModelError("Birthplace", "Aktualna wartość: " + PeopleInfo_DBValues.Birthplace);
                            if (PeopleInfo_DBValues.Height != people.PeopleInfo.Height)
                                ModelState.AddModelError("Height", "Aktualna wartość: " + PeopleInfo_DBValues.Height);
                            if (PeopleInfo_DBValues.Biography != people.PeopleInfo.Biography)
                                ModelState.AddModelError("Biography", "Aktualna wartość:  " + PeopleInfo_DBValues.Biography);

                            ViewData["Error"] = "Postać, którą próbujesz zmienić, została już zmieniona, jeśli nadal chcesz kontynuować, zapisz ponownie, lecz utracisz poprzednie dane.";
                            people.PeopleInfo.RowVersion = PeopleInfo_DBValues.RowVersion;
                        }
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
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
                RowVersion = people.PeopleInfo.RowVersion,
            };

            if (String.IsNullOrEmpty(ViewData["Error"].ToString()))
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
                TempData["Warning"] = "Dany film, nie istnieje";
                return RedirectToAction("Index");
            }
            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            { 
                People people = db.Peoples.Include(p => p.PeopleInfo).Include(p => p.Proffesion).FirstOrDefault(p => p.PeopleID == id);
                db.Peoples.Remove(people);

                IQueryable userRates = db.UserRates.Where(ur => ur.PeopleID == id);
                foreach (UserRate ur in userRates)
                    db.UserRates.Remove(ur);

                IQueryable photos = db.Photos.Where(p => p.PeopleID == id);
                foreach (Photo p in photos)
                {
                    if (p.MovieID == null)
                        db.Photos.Remove(p);
                    else
                    {
                        p.PeopleID = null;
                        p.ActorRole = null;
                    }
                }

                IQueryable actorroles = db.ActorRoles.Where(a => a.PeopleID == id);
                foreach (ActorRole a in actorroles)
                    db.ActorRoles.Remove(a);

                IQueryable movieinfo = db.MovieInfos.Where(d => d.DirectorID == id);
                foreach (MovieInfo m in movieinfo)
                {
                    m.DirectorID = null;
                    db.Entry(m).State = EntityState.Modified;
                }

                db.SaveChanges();
                TempData["Success"] = "Poprawnie usunięto postać filmową.";
            }
            catch (DataException)
            {
                TempData["Warning"] = "Nie można usunąć, spróbuj ponownie!";
                return RedirectToAction("Delete", new { id = id });
            }

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
        /* DO USUNIECIA
        public FileContentResult GetImage(int peopleId)
        {
            
            People people = db.Peoples.FirstOrDefault(p => p.PeopleID == peopleId);
            if (people != null)
            {
                return File(people.FacePhoto, people.FaceMimeType);
            }
            else
            {
                
            }
            
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
