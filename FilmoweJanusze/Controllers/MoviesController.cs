using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using PagedList;

using FilmoweJanusze.DAL;
using FilmoweJanusze.Models;
using FilmoweJanusze.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace FilmoweJanusze.Controllers
{
    public class MoviesController : ExtendedController
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public ActionResult Index(string sortParam, string sortOrder, string genre, string countryProduction, string years, int? pageSize, int? page)
        {
            IEnumerable<Movie> movies = db.Movies.Include(m => m.Genre).Include(m=>m.MovieInfo).Include(m => m.UserRates).ToList();

            //lista lat z dostepnych filmow
            ViewBag.Years = new SelectList(movies.OrderBy(m => m.ReleaseDate.Year).Select(m => m.ReleaseDate.Year).Distinct().ToList(), years);

            //listy sortujacefiltrujace
            ViewBag.SortParam = new SelectList(new[] { "Tytułu", "Daty premiery", "Oceny" }, sortParam);
            ViewBag.SortOrder = new SelectList(SortOrder(), sortOrder);
            ViewBag.Genre = new SelectList(MovieGenre.GetTypes(),genre);
            ViewBag.CountryProduction = new SelectList(CountryList(), countryProduction);
            ViewBag.PageSize = new SelectList(PageSizes(), pageSize);
            ViewBag.NoContent = "Brak filmów spełniających warunek :(";

            int pagesize = (pageSize ?? DefPageSize);
            int pageNumber = (page ?? DefPageNo);

            movies = SwitchGenre(movies, genre);
            movies = FilterCountry(movies, countryProduction);
            movies = FilterYears(movies, years);
            movies = SortMovies(movies, sortParam, sortOrder);

            if (Request.IsAjaxRequest())
                return PartialView("_TileList", movies.ToPagedList(pageNumber, pagesize));
            
            return View(movies.ToPagedList(pageNumber,pagesize));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {

            CheckID(id);

            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m=>m.Genre).Include(m => m.UserRates).Include("Cast.People").FirstOrDefault(m=>m.MovieID == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            //pobranie przykładowych zdjęć
            movie.Photos = db.Photos.Where(p => p.MovieID == id).Take(6).ToList();
            //id usera
            ViewBag.UserID = User.Identity.GetUserId();
            //liczba zdjęć w galerii
            ViewBag.PhotoCount = db.Photos.Where(p => p.MovieID == id).Count();

            //pobranie oceny zalogowanego usera
            if (movie.UserRates.Count > 0)
            {
                ViewBag.Rate = Math.Round(movie.UserRates.Average(ur=>ur.Rate),2);
                ViewBag.Controller = movie.Controller;
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            ViewBag.DirectorID = new SelectList(db.Peoples.Where(p => p.Proffesion.Director == true), "PeopleID", "FullName");
            ViewBag.CountryProduction = new SelectList(CountryList());
            ViewBag.DurationTimeValue = "00:00";

            MovieFormView movieFormView = new MovieFormView();
            movieFormView.PhotoURL = "";

            return View(movieFormView);
        }

        // POST: Movies/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,TitlePL,ReleaseDate,Genre,Description,DirectorID,Genre,TrailerURL,CountryProduction,DurationTime,PhotoURL")] MovieFormView movieFormView, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image)
        {
            ViewBag.DirectorID = new SelectList(db.Peoples.Where(p => p.Proffesion.Director == true), "PeopleID", "FullName", movieFormView.DirectorID);
            ViewBag.CountryProduction = new SelectList(CountryList(), movieFormView.CountryProduction);
            ViewBag.DurationTimeValue = movieFormView.DurationTime.ToShortTimeString();

            if (ModelState.IsValid)
            {

                Movie movie = new Movie
                {
                    Title = movieFormView.Title,
                    TitlePL = movieFormView.TitlePL,
                    ReleaseDate = movieFormView.ReleaseDate,
                    Genre = movieFormView.Genre,
                };

                movie.MovieInfo = new MovieInfo
                {
                    CountryProduction = movieFormView.CountryProduction,
                    Description = movieFormView.Description,
                    DirectorID = movieFormView.DirectorID,
                    DurationTime = movieFormView.DurationTime,
                    TrailerURL = movieFormView.TrailerURL
                };


                if (!String.IsNullOrEmpty(RadioPhotoBtn))
                {
                    switch (RadioPhotoBtn)
                    {
                        case "FromFile":
                            if (image != null)
                                movie.PhotoURL = SaveNewFile("MoviePosters", movie.TitleYear, image);
                            else
                                movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                            break;
                        case "FromURL":
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                DeleteOldFile(movie.PhotoURL);
                                movie.PhotoURL = UrlPath;
                            }
                            break;
                        case "None":
                            DeleteOldFile(movie.PhotoURL);
                            movie.PhotoURL = NoContentPhoto;
                            break;
                        default:
                            movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                            break;
                    }
                }
                else
                {
                    movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                }
                /*
                if (movie.Genre.Count() == 0)
                    movie.Genre = null;
                    */
                movie.MovieInfo.TrailerURL = ModifyTrailerURL(movie.MovieInfo.TrailerURL);

                db.Movies.Add(movie);
                db.SaveChanges();
                TempData["Success"] = "Poprawnie dodano film.";
                return RedirectToAction("Index");
            }
            ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(movieFormView);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).FirstOrDefault();
            if (movie == null)
            {
                return HttpNotFound();
            }

            MovieFormView movieFormView = new MovieFormView
            {
                Title = movie.Title,
                TitlePL = movie.TitlePL,
                ReleaseDate = movie.ReleaseDate,
                PhotoURL = movie.PhotoURL,
                Genre = movie.Genre,

                CountryProduction = movie.MovieInfo.CountryProduction,
                Description = movie.MovieInfo.Description,
                DirectorID = movie.MovieInfo.DirectorID,
                DurationTime = movie.MovieInfo.DurationTime,
                TrailerURL = movie.MovieInfo.TrailerURL,
                RowVersion = movie.MovieInfo.RowVersion,
                
            };

            ViewBag.DirectorID = new SelectList(db.Peoples.Where(p => p.Proffesion.Director == true), "PeopleID", "FullName", movieFormView.DirectorID);
            ViewBag.CountryProduction = new SelectList(CountryList(), movieFormView.CountryProduction);
            ViewBag.DurationTimeValue = movieFormView.DurationTime.ToShortTimeString();
            ViewBag.Name = movie.TitleYear;
            ViewBag.MovieID = id;

            return View(movieFormView);
        }

        // POST: Movies/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Edit(int? id, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image, byte[] rowVersion)
        {
            //[Bind(Include = "MovieID,Title,TitlePL,ReleaseDate,Genre,Description,Poster,PosterMimeType,DirectorID")] Movie movie

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).FirstOrDefault();
            if(movie == null)
            {
                return HttpNotFound();
            }

            if (TryUpdateModel(movie, "", new string[] { "Title", "TitlePL", "ReleaseDate", "Genre" }))
            {
                if(TryUpdateModel(movie.MovieInfo,"", new string[] { "Description", "DirectorID", "TrailerURL", "CountryProduction", "DurationTime" , "RowVersion"}))
                { 
                    try
                    {

                        if (ModelState.IsValid)
                        {
                            /* DELETE
                            if (image != null){
                                movie.PosterMimeType = image.ContentType;
                                movie.Poster = new byte[image.ContentLength];
                                image.InputStream.Read(movie.Poster, 0, image.ContentLength);
                            }*/

                            db.Entry(movie.MovieInfo).OriginalValues["RowVersion"] = rowVersion;

                            if (!String.IsNullOrEmpty(RadioPhotoBtn))
                            {
                                switch (RadioPhotoBtn)
                                {
                                    case "FromFile":
                                        if (image != null)
                                            movie.PhotoURL = SaveNewFile("MoviePosters", movie.TitleYear, image);
                                        else
                                            movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                                        break;
                                    case "FromURL":
                                        if (!String.IsNullOrEmpty(UrlPath))
                                        {
                                            DeleteOldFile(movie.PhotoURL);
                                            movie.PhotoURL = UrlPath;
                                        }
                                        break;
                                    case "None":
                                        DeleteOldFile(movie.PhotoURL);
                                        movie.PhotoURL = NoContentPhoto;
                                        break;
                                    default:
                                        movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                                        break;
                                }
                            }
                            else
                            {
                                movie.PhotoURL = IfEmptySetEmptyPhoto(movie.PhotoURL);
                            }
                            /*
                            if (movie.MovieInfo.Genre.Count() == 0)
                            {
                                movie.MovieInfo.Genre = null;
                            }
                            */
                            movie.MovieInfo.TrailerURL = ModifyTrailerURL(movie.MovieInfo.TrailerURL);

                            db.Entry(movie).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["Success"] = "Poprawnie zmieniono informacje o filmie.";
                            return RedirectToAction("Details", "Movies", new { id = movie.MovieID });
                        }
                    }
                    catch(DbUpdateConcurrencyException ex)
                    {
                       // var DB_MovieID = ex.Entries.Single().Property("MovieID").OriginalValue;
                        var MovieInfo_DBEntry = ex.Entries.Single().GetDatabaseValues();
                        
                        if (MovieInfo_DBEntry == null)
                        {
                            ModelState.AddModelError(string.Empty,"Film, który próbujesz zmienić, został już usunięty.");
                        }
                        else
                        {
                            var MovieInfo_DBValues = (MovieInfo)MovieInfo_DBEntry.ToObject();
                            var Movie_DBValues = (Movie)db.Entry(movie).GetDatabaseValues().ToObject();
                            var Genre_DBValues = (MovieGenre)db.Entry(movie.Genre).GetDatabaseValues().ToObject();

                            if (Movie_DBValues.Title != movie.Title)
                                ModelState.AddModelError("Title", "Aktualna wartość: " + Movie_DBValues.Title);
                            if (Movie_DBValues.TitlePL != movie.TitlePL)
                                ModelState.AddModelError("TitlePL", "Aktualna wartość: " + Movie_DBValues.TitlePL);
                            if (Movie_DBValues.ReleaseDate != movie.ReleaseDate)
                                ModelState.AddModelError("ReleaseDate", "Aktualna wartość: " + Movie_DBValues.ReleaseDate.ToShortDateString());
                            if (Movie_DBValues.PhotoURL != movie.PhotoURL)
                                ModelState.AddModelError(String.Empty, "Zmieniony został plakat filmu");

                            if(!Genre_DBValues.EqualTo(movie.Genre))
                                ModelState.AddModelError(String.Empty, "Zmieniony został gatunek filmu");

                            if (MovieInfo_DBValues.CountryProduction != movie.MovieInfo.CountryProduction)
                                ModelState.AddModelError("CountryProduction", "Aktualna wartość: " + MovieInfo_DBValues.CountryProduction);
                            if (MovieInfo_DBValues.Description != movie.MovieInfo.Description)
                                ModelState.AddModelError("Description", "Aktualna wartość: " + MovieInfo_DBValues.Description);
                            if (MovieInfo_DBValues.DirectorID != movie.MovieInfo.DirectorID)
                                ModelState.AddModelError("DirectorID", "Aktualna wartość: " + db.Peoples.Where(p => p.PeopleID == MovieInfo_DBValues.DirectorID).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault());
                            if (MovieInfo_DBValues.DurationTime != movie.MovieInfo.DurationTime)
                            {
                                ModelState.AddModelError("DurationTime", "Aktualna wartość czasu trwania:  " + MovieInfo_DBValues.DurationTime.ToShortTimeString());
                                ModelState.AddModelError(String.Empty, "Aktualna wartość czasu trwania:  " + MovieInfo_DBValues.DurationTime.ToShortTimeString());
                            }
                                
                            if (MovieInfo_DBValues.TrailerURL != movie.MovieInfo.TrailerURL)
                                ModelState.AddModelError("TrailerURL", "Aktualna wartość: " + MovieInfo_DBValues.TrailerURL);

                            ViewData["Error"] = "Film, który próbujesz zmienić, został już zmieniony, jeśli nadal chcesz kontynuować, zapisz ponownie, lecz utracisz poprzednie dane.";
                            movie.MovieInfo.RowVersion = MovieInfo_DBValues.RowVersion;
                        }
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
            }

            ViewBag.DirectorID = new SelectList(db.Peoples.Where(p => p.Proffesion.Director == true), "PeopleID", "FullName", movie.MovieInfo.DirectorID);
            ViewBag.CountryProduction = new SelectList(CountryList(), movie.MovieInfo.CountryProduction);
            ViewBag.DurationTimeValue = movie.MovieInfo.DurationTime.ToShortTimeString();
            ViewBag.Name = movie.TitleYear;
            ViewBag.MovieID = id;

            MovieFormView movieFormView = new MovieFormView
            {
                Title = movie.Title,
                TitlePL = movie.TitlePL,
                ReleaseDate = movie.ReleaseDate,
                PhotoURL = movie.PhotoURL,
                Genre = movie.Genre,

                CountryProduction = movie.MovieInfo.CountryProduction,
                Description = movie.MovieInfo.Description,
                DirectorID = movie.MovieInfo.DirectorID,
                DurationTime = movie.MovieInfo.DurationTime,
                TrailerURL = movie.MovieInfo.TrailerURL,
                RowVersion = movie.MovieInfo.RowVersion,
            };
            if(String.IsNullOrEmpty(ViewData["Error"].ToString()))
                ViewData["Error"] = "Nie można zapisać, popraw błędy!";
            return View(movieFormView);
        }



        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Include(m=>m.MovieInfo).Where(m=>m.MovieID == id).FirstOrDefault();

            if (movie == null)
            {
                TempData["Warning"] = "Dany film, nie istnieje";
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).FirstOrDefault();
                db.Movies.Remove(movie);

                IQueryable actorroles = db.ActorRoles.Where(a => a.MovieID == id);
                foreach (ActorRole a in actorroles)
                    db.ActorRoles.Remove(a);

                IQueryable userRates = db.UserRates.Where(ur => ur.MovieID == id);
                foreach (UserRate ur in userRates)
                    db.UserRates.Remove(ur);

                IQueryable photos = db.Photos.Where(p => p.MovieID == id);
                foreach (Photo p in userRates)
                {
                    if (p.PeopleID == null)
                        db.Photos.Remove(p);
                    else
                        p.MovieID = null;
                }

                db.SaveChanges();
                TempData["Success"] = "Poprawnie usunięto film.";
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
        /*
        public FileContentResult GetImage(int movieId )
        {
            
            Movie movie = db.Movies.FirstOrDefault(p => p.MovieID == movieId);
            if (movie != null)
            {
                return File(movie.Poster, movie.PosterMimeType);
            }
            else
            {
                return null;
            }
            
            return null;
        }
        */

        public static string ModifyTrailerURL(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                int index = url.IndexOf("watch?v=");
                if (index > 0)
                    return url.Substring(0, index) + "embed/" + url.Substring(index + 8, url.Length - index - 8);
                else
                    return url;
            }
            else
                return null;
        }

        [HttpPost]
        public JsonResult ValidateMovieGenreCount(string Genre)
        {

            if (Genre != null)
            {
                if (Genre.Count() > 3)
                    return Json(false);
            }
            return Json(true);
        }
    }
}

