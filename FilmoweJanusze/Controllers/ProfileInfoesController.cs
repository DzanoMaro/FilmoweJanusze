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
using Microsoft.AspNet.Identity;

namespace FilmoweJanusze.Controllers
{
    [Authorize]
    public class ProfileInfoesController : ExtendedController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProfileInfoes
        /*
        public ActionResult Index()
        {
            //id usera
            ViewBag.UserID = User.Identity.GetUserId();
            return View(profileInfo);
        }
        */

        // GET: ProfileInfoes/Details/5
        public ActionResult Details(string UserName)
        {
            if (String.IsNullOrEmpty(UserName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileInfo profileInfo = db.ProfileInfos.FirstOrDefault(p=>p.User.UserName == UserName);
            if (profileInfo == null)
            {
                return RedirectToAction("Create");
            }
            return View(profileInfo);
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
                return RedirectToAction("Details",new { UserName = profileInfo.User.UserName });
            }

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
                return RedirectToAction("Details", new { UserName = profileInfo.User.UserName });
            }
            return View(profileInfo);
        }

        // GET: ProfileInfoes/Delete/5
        public ActionResult Delete(int? id)
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

            ViewBag.CurrentUserID = User.Identity.GetUserId();

            return View(profileInfo);
        }

        // POST: ProfileInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProfileInfo profileInfo = db.ProfileInfos.Find(id);
            db.ProfileInfos.Remove(profileInfo);
            db.SaveChanges();
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
    }
}
