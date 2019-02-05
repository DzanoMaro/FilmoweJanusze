using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmoweJanusze.Models;
using FilmoweJanusze.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace FilmoweJanusze.Controllers
{
    [Authorize]
    public class ProfileInfoesController : ExtendedController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProfileInfoes/Details/5
        public ActionResult Details(string UserName)
        {
            if (String.IsNullOrEmpty(UserName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileInfoesDetails profileInfoesDetails = new ProfileInfoesDetails();

            profileInfoesDetails.ProfileInfo = db.ProfileInfos.Include(pi=>pi.User).FirstOrDefault(p=>p.User.UserName == UserName);
            if (profileInfoesDetails.ProfileInfo == null)
            {
                return RedirectToAction("Create");
            }

            ViewBag.CurrentUserID = User.Identity.GetUserId();
            profileInfoesDetails.RatedMovies = db.UserRates.Include(u=>u.Movie).Where(u => u.User.Id == profileInfoesDetails.ProfileInfo.User.Id && u.MovieID != null).Take(4 * TilesPerCarousel).ToList();
            profileInfoesDetails.RatedPeoples = db.UserRates.Include(u => u.People).Where(u => u.User.Id == profileInfoesDetails.ProfileInfo.User.Id && u.PeopleID != null).Take(4 * TilesPerCarousel).ToList();

            return View(profileInfoesDetails);
        }

        // GET: ProfileInfoes/Create
        public ActionResult Create()
        {
            var UserID = User.Identity.GetUserId();
            ProfileInfo profileInfo = db.ProfileInfos.FirstOrDefault(p => p.UserID == UserID);
            if (profileInfo != null)
            {
                RedirectToAction("Edit", profileInfo.UserID);
            }

            profileInfo = new ProfileInfo();
            profileInfo.PhotoURL = "";
            profileInfo.Birthdate = DateTime.Now;
            return View(profileInfo);
        }

        // POST: ProfileInfoes/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Birthdate")] ProfileInfo profileInfo, HttpPostedFileBase image)
        {
            var UserID = User.Identity.GetUserId();

            profileInfo.User = db.Users.Find(UserID);

            //CheckBirthday(profileInfo.Birthdate);

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string root = Server.MapPath("~/");
                    string folder = "/Images/Profiles/";
                    string name = profileInfo.User.UserName;
                    string ext = System.IO.Path.GetExtension(image.FileName);

                    //tworzy folder
                    System.IO.Directory.CreateDirectory(root + folder);

                    //crop
                    Image crop = Image.FromStream(image.InputStream, true, true);
                    SaveCroppedImage(crop, 300, 300, root + folder + name + ext);
                    profileInfo.PhotoURL = folder + name + ext;
                }

                db.ProfileInfos.Add(profileInfo);
                db.SaveChanges();

                

                TempData["Success"] = "Poprawnie dodano informacje profilowe.";
                return RedirectToAction("Details",new { UserName = profileInfo.User.UserName });
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(profileInfo);
        }

        // GET: ProfileInfoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileInfo profileInfo = db.ProfileInfos.Include(pi => pi.User).FirstOrDefault(pi => pi.UserID == id);
            if (profileInfo == null)
            {
                return HttpNotFound();
            }
            return View(profileInfo);
        }

        // POST: ProfileInfoes/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Birthdate")] ProfileInfo profileInfo, HttpPostedFileBase image)
        {
            //CheckBirthday(profileInfo.Birthdate);
            if (ModelState.IsValid)
            {
                profileInfo.User = db.Users.Find(profileInfo.UserID);

                if (image != null)
                {
                    string root = Server.MapPath("~/");
                    string folder = "/Images/Profiles/";
                    string name = profileInfo.User.UserName;
                    string ext = System.IO.Path.GetExtension(image.FileName);

                    string path = Server.MapPath("~/") + profileInfo.PhotoURL;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    //crop
                    Image crop = Image.FromStream(image.InputStream, true, true);
                    SaveCroppedImage(crop, 300, 300, root + folder + name + ext);
                    profileInfo.PhotoURL = folder + name + ext;
                }

                db.Entry(profileInfo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Poprawnie zmieniono informacje profilowe.";
                return RedirectToAction("Details", new { UserName = profileInfo.User.UserName });
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(profileInfo);
        }

        /* TODO */
        public ActionResult Index(string id, string rate, string genre, string proffesion, int? pageSize, int? page)
        {
            CheckID(id);

            var UserID = User.Identity.GetUserId();
            ViewBag.ID = id;
            ViewBag.UserID = UserID;
            ViewBag.PageSize = new SelectList(PageSizes(), pageSize);
            ViewBag.Genre = new SelectList(MovieGenre.GetTypes(), genre);
            ViewBag.Proffesion = new SelectList(Proffesion.GetProffesions(), proffesion);
            ViewBag.Rate = new SelectList(UserRate.GetRateRange(), rate);

            ViewBag.ProfileInfoes = true;
            ViewBag.RatesCount = new int[UserRate.GetRateRange().Count()];

            int pagesize = (pageSize ?? DefPageSize);
            int pageNumber = (page ?? DefPageNo);

            if (id == "Movies")
            {
                ViewBag.NoContent = "Brak filmów spełniających warunek :(";
                ViewBag.Controller = "Movies";
                
                IEnumerable<Movie> movies = db.Movies.Include(m=>m.Genre).Include(m => m.UserRates).Where(m => m.UserRates.Any(ur=>ur.UserID == UserID)).ToList();
                ViewBag.Count = movies.Count();

                for(int i = 0; i < UserRate.GetRateRange().Count(); i++)
                {
                    ViewBag.RatesCount[i] = movies.Where(t => t.UserRates.Any(ur => ur.Rate == i+1 && ur.UserID == ViewBag.UserID)).Count();
                }

                movies = SwitchGenre(movies, genre);
                movies = FilterRate(movies, rate, UserID);

                if (Request.IsAjaxRequest())
                    return PartialView("_TileList", movies.ToPagedList(pageNumber, pagesize));

                return View(movies.ToPagedList(pageNumber, pagesize));
            }
            else if (id == "People")
            {
                ViewBag.NoContent = "Brak postaci kina spełniających warunek :(";
                ViewBag.Controller = "People";

                IEnumerable<People> peoples = db.Peoples.Include(p=>p.Proffesion).Include(p => p.UserRates).Where(p => p.UserRates.Any(ur => ur.UserID == UserID)).ToList();
                ViewBag.Count = peoples.Count();

                for (int i = 0; i < UserRate.GetRateRange().Count(); i++)
                {
                    ViewBag.RatesCount[i] = peoples.Where(t => t.UserRates.Any(ur => ur.Rate == i + 1 && ur.UserID == ViewBag.UserID)).Count();
                }

                peoples = SwitchProffesion(peoples, proffesion);
                peoples = FilterRate(peoples, rate, UserID);

                if (Request.IsAjaxRequest())
                    return PartialView("_TileList", peoples.ToPagedList(pageNumber, pagesize));

                return View(peoples.ToPagedList(pageNumber, pagesize));
            }
            else 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
