using FilmoweJanusze.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Controllers
{
    public class PhotoController : ExtendedController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photo
        public ActionResult Index(int? movieID, int? peopleID)
        {
            if ((movieID == null && peopleID == null) || (movieID != null && peopleID != null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ICollection<Photo> photos = null;

            if (movieID != null)
            {
                photos = db.Photos.Where(p => p.MovieID == movieID).ToList();
                if(photos.Count == 0)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = movieID;
                ViewBag.PeopleID = null;
                ViewBag.Name = photos.FirstOrDefault().Movie.TitleYear;
            }

            if (peopleID != null)
            {
                photos = db.Photos.Where(p => p.PeopleID == peopleID).ToList();
                if (photos.Count == 0)
                {
                    return HttpNotFound();
                }
                ViewBag.PeopleID = peopleID;
                ViewBag.MovieID = null;
                ViewBag.Name = photos.FirstOrDefault().People.FullName;
            }

            return View(photos);
        }

        public ActionResult Details(int? movieID, int? peopleID, int? photoID)
        {
            if((movieID == null && peopleID == null) || (movieID != null && peopleID != null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ICollection<Photo> photos = null;

            if (movieID != null)
            {
                photos = db.Photos.Where(p => p.MovieID == movieID).ToList();
                if (photos.Count == 0)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = movieID;
                ViewBag.PeopleID = null;
                ViewBag.Name = photos.FirstOrDefault().Movie.TitleYear;
            }
            else
            if (peopleID != null)
            {
                photos = db.Photos.Where(p => p.PeopleID == peopleID).ToList();
                if (photos.Count == 0)
                {
                    return HttpNotFound();
                }
                ViewBag.PeopleID = peopleID;
                ViewBag.MovieID = null;
                ViewBag.Name = photos.FirstOrDefault().People.FullName;
            }

            if (photoID != null)
            {
                ViewBag.PhotoID = photoID;
            }

            if (photos.Count == 0)
            {
                return HttpNotFound();
            }

            return View(photos);
        }


        [Authorize(Roles = "User,Admin")]
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
                ViewBag.PeopleID = null;
                ViewBag.Name = movie.TitleYear;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.MovieID == movieID).ToList(), "ActorRoleID", "FullRoleName");
            }

            if (peopleID != null)
            {
                People people = db.Peoples.Find(peopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PeopleID = peopleID;
                ViewBag.MovieID = null;
                ViewBag.Name = people.FullName;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.PeopleID == peopleID).ToList(), "ActorRoleID", "RoleMovie");
            }

            return View();
        }

        // POST: ActorRoles/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Create([Bind(Include = "PhotoID,MovieID,PeopleID,ActorRoleID")] Photo photo, HttpPostedFileBase image, bool? ismovie, bool? ispeople)
        {
            if ((photo.MovieID == null && photo.PeopleID == null) || (photo.MovieID != null && photo.PeopleID != null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (photo.MovieID != null)
            {
                Movie movie = db.Movies.Find(photo.MovieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MovieID = photo.MovieID;
                ViewBag.PeopleID = null;
                ViewBag.Name = movie.TitleYear;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.MovieID == photo.MovieID).ToList(), "ActorRoleID", "FullRoleName", photo.ActorRoleID);
            }

            if (photo.PeopleID != null)
            {
                People people = db.Peoples.Find(photo.PeopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PeopleID = photo.PeopleID;
                ViewBag.MovieID = null;
                ViewBag.Name = people.FullName;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.PeopleID == photo.PeopleID).ToList(), "ActorRoleID", "RoleMovie", photo.ActorRoleID);
            }

            if (ModelState.IsValid)
            {
                if (photo.ActorRoleID!=null)
                {
                    photo.ActorRole = db.ActorRoles.Find(photo.ActorRoleID);
                    photo.MovieID = photo.ActorRole.MovieID;
                    photo.PeopleID = photo.ActorRole.PeopleID;
                }

                if (image != null)
                {
                    string root = Server.MapPath("~/");
                    string folder = "/Images/" + ViewBag.Name + "/";
                    string thumbfolder = "/Images/Thumb/" + ViewBag.Name + "/";
                    string name = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                    string ext = System.IO.Path.GetExtension(image.FileName);
                    
                    //tworzy folder dla oryginału
                    System.IO.Directory.CreateDirectory(root+folder);
                    //tworzy folder dla thumbnails
                    System.IO.Directory.CreateDirectory(root+thumbfolder);

                    //jeśli istnieje już takie zdjęcie
                    if (System.IO.File.Exists(root + folder + name + ext))
                    {
                        return View();
                    }

                    //ścieżka lokalna root jako Projekt
                    photo.PhotoURL = folder + name + ext;
                    photo.PhotoThumbURL = thumbfolder + name + ext;
                    //zapisanie zdjecia wg sciezki do konkretnego miejsca na dysku
                    image.SaveAs(root+folder+name+ext);
                    //stworzenie thumbnail
                    Image thumb = Image.FromStream(image.InputStream, true, true);
                    SaveCroppedImage(thumb, 300, 400, root+thumbfolder+name+ext);

                    db.Photos.Add(photo);
                    db.SaveChanges();

                    if (ismovie == true)
                    {
                        return RedirectToAction("Index", "Photo", new { movieID = photo.MovieID });
                    }
                    else if (ispeople == true)
                    {
                        return RedirectToAction("Index", "Photo", new { peopleID = photo.PeopleID });
                    }

                }
                return View();
            }

            return View();
        }

        // GET: ActorRoles/Edit/5
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int? id, bool? ismovie, bool? ispeople)
        {
            if ((id == null) || (ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Photo photo = db.Photos.Where(p=>p.PhotoID == id).Single();
            if (photo == null)
            {
                return HttpNotFound();
            }

            if (ismovie == true)
            {
                ViewBag.PeopleID = null;
                ViewBag.MovieID = true;
                ViewBag.Name = photo.Movie.TitleYear;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.MovieID == photo.MovieID).ToList(), "ActorRoleID", "FullRoleName", photo.ActorRoleID);
            }

            if (ispeople == true)
            {
                ViewBag.PeopleID = true;
                ViewBag.MovieID = null;
                ViewBag.Name = photo.People.FullName;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.PeopleID == photo.PeopleID).ToList(), "ActorRoleID", "RoleMovie", photo.ActorRoleID);
            }

            return View(photo);
        }

        // POST: ActorRoles/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit([Bind(Include = "PhotoID,MovieID,PeopleID,PhotoURL,PhotoThumbURL,ActorRoleID")] Photo photo, bool? ismovie, bool? ispeople)
        {
            if ((ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (photo == null)
            {
                return HttpNotFound();
            }

            if (photo.MovieID != null)
            {
                Movie movie = db.Movies.Find(photo.MovieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                photo.Movie = movie;
                ViewBag.MovieID = photo.MovieID;
                ViewBag.PeopleID = null;
                ViewBag.Name = photo.Movie.TitleYear;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.MovieID == photo.MovieID).ToList(), "ActorRoleID", "FullRoleName", photo.ActorRoleID);
            }

            if (photo.PeopleID != null)
            {
                People people = db.Peoples.Find(photo.PeopleID);
                if (people == null)
                {
                    return HttpNotFound();
                }
                photo.People = people;
                ViewBag.PeopleID = photo.PeopleID;
                ViewBag.MovieID = null;
                ViewBag.Name = photo.People.FullName;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.PeopleID == photo.PeopleID).ToList(), "ActorRoleID", "RoleMovie", photo.ActorRoleID);
            }

            if (ModelState.IsValid)
            {
                if (photo.ActorRoleID != null)
                {
                    photo.ActorRole = db.ActorRoles.Find(photo.ActorRoleID);
                    photo.MovieID = photo.ActorRole.MovieID;
                    photo.PeopleID = photo.ActorRole.PeopleID;
                } else
                {
                    if( ismovie == true )
                    {
                        photo.PeopleID = null;
                        photo.People = null;
                    }
                    else if( ispeople == true )
                    {
                        photo.MovieID = null;
                        photo.Movie = null;
                    }

                }

                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();

                if (ismovie == true)
                {
                    return RedirectToAction("Index", "Photo", new { movieID = photo.MovieID });
                }
                else if (ispeople == true)
                {
                    return RedirectToAction("Index", "Photo", new { peopleID = photo.PeopleID });
                }
            }

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

            Photo photo = db.Photos.Where(p => p.PhotoID == id).Single();
            if (photo == null)
            {
                return HttpNotFound();
            }

            if (ismovie == true)
            {
                ViewBag.PeopleID = null;
                ViewBag.MovieID = true;
                ViewBag.Name = photo.Movie.TitleYear;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.MovieID == photo.MovieID).ToList(), "ActorRoleID", "FullRoleName", photo.ActorRoleID);
            }
            else
            if (ispeople == true)
            {
                ViewBag.PeopleID = true;
                ViewBag.MovieID = null;
                ViewBag.Name = photo.People.FullName;
                ViewBag.ActorRoleID = new SelectList(db.ActorRoles.Where(p => p.PeopleID == photo.PeopleID).ToList(), "ActorRoleID", "RoleMovie", photo.ActorRoleID);
            }

            return View(photo);
        }

        // POST: ActorRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int? id, bool? ismovie, bool? ispeople)
        {

            if ((id == null) || (ismovie == null && ispeople == null) || (ismovie == false && ispeople == false) || (ismovie == true && ispeople == true))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            //usuniecie zdjecia z dyski
            string path = Server.MapPath("~/") + photo.PhotoURL;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            //usuniecie miniaturki z dysku
            path = Server.MapPath("~/") + photo.PhotoThumbURL;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            int redirectid = 0;
            if (ismovie == true)
            {
                redirectid = photo.MovieID.Value;
            }
            else if (ispeople == true)
            {
                redirectid = photo.PeopleID.Value;
            }

            db.Photos.Remove(photo);
            db.SaveChanges();

            if (ismovie == true)
            {
                return RedirectToAction("Index", "Photo", new { movieID = redirectid });
            }
            else if (ispeople == true)
            {
                return RedirectToAction("Index", "Photo", new { peopleID = redirectid });
            }

            return RedirectToAction("Index","Home");
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
