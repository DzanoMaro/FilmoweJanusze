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

            profileInfoesDetails.ProfileInfo = db.ProfileInfos.FirstOrDefault(p=>p.User.UserName == UserName);
            if (profileInfoesDetails.ProfileInfo == null)
            {
                return RedirectToAction("Create");
            }

            ViewBag.CurrentUserID = User.Identity.GetUserId();
            profileInfoesDetails.RatedMovies = db.UserRates.Include(u=>u.Movie).Where(u => u.User.Id == profileInfoesDetails.ProfileInfo.User.Id).ToList();

            return View(profileInfoesDetails);
        }

        // GET: ProfileInfoes/Create
        public ActionResult Create()
        {
            string UserID = User.Identity.GetUserId();

            ProfileInfo profileInfo = db.ProfileInfos.FirstOrDefault(p => p.User.Id == UserID);
            if (profileInfo != null)
            {
                RedirectToAction("Edit", profileInfo.ProfileInfoID);
            }

            profileInfo = new ProfileInfo();
            profileInfo.User = db.Users.Find(UserID);
            profileInfo.PhotoURL = "";
            profileInfo.Birthdate = DateTime.Now;
            return View(profileInfo);
        }

        // POST: ProfileInfoes/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileInfoID,FirstName,LastName,Birthdate,User")] ProfileInfo profileInfo, HttpPostedFileBase image)
        {
            profileInfo.User = db.Users.Find(profileInfo.User.Id);

            CheckBirthday(profileInfo.Birthdate);

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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileInfo profileInfo = db.ProfileInfos.Find(id);
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
        public ActionResult Edit([Bind(Include = "ProfileInfoID,FirstName,LastName,Birthdate,User")] ProfileInfo profileInfo, HttpPostedFileBase image)
        {
            profileInfo.User = db.Users.Find(profileInfo.User.Id);
            CheckBirthday(profileInfo.Birthdate);
            if (ModelState.IsValid)
            {
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
