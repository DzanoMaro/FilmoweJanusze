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
            IQueryable<Movie> movies = db.Movies.Include(m=>m.MovieInfo);

            switch (genre)
            {
                case "Akcja":
                    movies = movies.Where(m => m.Genre.Action == true);
                    break;
                case "Animowany":
                    movies = movies.Where(m => m.Genre.Anime == true);
                    break;
                case "Biograficzny":
                    movies = movies.Where(m => m.Genre.Biographic == true);
                    break;
                case "Dokumentalny":
                    movies = movies.Where(m => m.Genre.Documental == true);
                    break;
                case "Dramat":
                    movies = movies.Where(m => m.Genre.Drama == true);
                    break;
                case "Familijny":
                    movies = movies.Where(m => m.Genre.Familly == true);
                    break;
                case "Fantasy":
                    movies = movies.Where(m => m.Genre.Fantasy == true);
                    break;
                case "Horror":
                    movies = movies.Where(m => m.Genre.Horror == true);
                    break;
                case "Komedia":
                    movies = movies.Where(m => m.Genre.Comedy == true);
                    break;
                case "Krótkometrażowy":
                    movies = movies.Where(m => m.Genre.Short == true);
                    break;
                case "Krotkometrażowy":    //dont delete
                    movies = movies.Where(m => m.Genre.Short == true);
                    genre = "Krótkometrażowy";
                    break;
                case "Kryminalny":
                    movies = movies.Where(m => m.Genre.Criminal == true);
                    break;
                case "Melodramat":
                    movies = movies.Where(m => m.Genre.Melodrama == true);
                    break;
                case "Musical":
                    movies = movies.Where(m => m.Genre.Musical == true);
                    break;
                case "Muzyczny":
                    movies = movies.Where(m => m.Genre.Music == true);
                    break;
                case "Przygodowy":
                    movies = movies.Where(m => m.Genre.Adventure == true);
                    break;
                case "Romans":
                    movies = movies.Where(m => m.Genre.Romans == true);
                    break;
                case "Sci-Fi":
                    movies = movies.Where(m => m.Genre.SciFi == true);
                    break;
                case "Thriller":
                    movies = movies.Where(m => m.Genre.Thriller == true);
                    break;
                default:
                    break;
            }
            
            //lista lat z dostepnych filmow
            ViewBag.Years = new SelectList(movies.Select(m => m.ReleaseDate.Year).Distinct().ToList(), years);

            //listy sortujacefiltrujace
            ViewBag.SortParam = new SelectList(new[] { "Tytułu", "Daty premiery" }, sortParam);
            ViewBag.SortOrder = new SelectList(new[] { "Rosnąco", "Malejąco" }, sortOrder);
            ViewBag.Genre = new SelectList(new[] { "Akcja", "Animowany", "Biograficzny", "Dokumentalny", "Dramat", "Familijny", "Fantasy", "Horror", "Komedia", "Krótkometrażowy", "Kryminalny", "Melodramat", "Musical", "Muzyczny", "Przygodowy", "Romans", "Sci-Fi", "Thriller" },genre);
            ViewBag.CountryProduction = new SelectList(CountryList(), countryProduction);
            ViewBag.PageSize = new SelectList(new[] { 4, 8, 12, 20 }, pageSize);

            int pagesize = (pageSize ?? 8);
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(countryProduction))
            {
                movies = movies.Where(m => m.MovieInfo.CountryProduction == countryProduction);
            }

            if(!String.IsNullOrEmpty(years))
            {
                movies = movies.Where(m => m.ReleaseDate.Year.ToString().Equals(years));
            }

            if (sortParam == "Daty premiery")
            {
                if (sortOrder == "Malejąco")
                    movies = movies.OrderByDescending(m=>m.ReleaseDate);
                else
                    movies = movies.OrderBy(m => m.ReleaseDate);
            }
            else
            {
                if (sortOrder == "Malejąco")
                    movies = movies.OrderByDescending(m => m.Title);
                else
                    movies = movies.OrderBy(m => m.Title);
            }

            if (Request.IsAjaxRequest())
                return PartialView("_MovieList", movies.ToPagedList(pageNumber, pagesize));
            
            return View(movies.ToPagedList(pageNumber,pagesize));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MovieandCast movieandCast = new MovieandCast();
            movieandCast.Movie = db.Movies.Include(m => m.MovieInfo).Include(m=>m.Genre).Include("Cast.People").FirstOrDefault(m=>m.MovieID == id);

            if (movieandCast.Movie == null)
            {
                return HttpNotFound();
            }

            //pobranie reżysera
            movieandCast.People = db.Peoples.Find(movieandCast.Movie.MovieInfo.DirectorID);
            //pobranie obsady
            movieandCast.UserRates = db.UserRates.Where(ur => ur.MovieID == id).ToList();
            //pobranie przykładowych zdjęć
            movieandCast.Photos = db.Photos.Where(p => p.MovieID == id).Take(6).ToList();

            //id usera
            ViewBag.UserID = User.Identity.GetUserId();
            //liczba zdjęć w galerii
            ViewBag.PhotoCount = db.Photos.Where(p => p.MovieID == id).Count();


            //pobranie oceny zalogowanego usera
            if (movieandCast.UserRates.Count > 0)
            {
                ViewBag.MovieRate = movieandCast.UserRates.Average(ur => ur.Rate);
                ViewBag.PeopleRate = null;
                if (ViewBag.UserID != null)
                {
                    movieandCast.LoggedInURate = movieandCast.UserRates.FirstOrDefault(ur => ur.UserID == ViewBag.UserID);
                }
            }

            //jeśli nie oceniono jeszcze
            if (movieandCast.LoggedInURate == null)
            {
                movieandCast.LoggedInURate = new UserRate();
                movieandCast.LoggedInURate.MovieID = (int)id;
                movieandCast.LoggedInURate.Movie = movieandCast.Movie;
                movieandCast.LoggedInURate.PeopleID = null;
                movieandCast.LoggedInURate.People = null;
            }

            return View(movieandCast);
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
        public ActionResult Create([Bind(Include = "MovieID,Title,TitlePL,ReleaseDate,Genre,Description,DirectorID,Genre,TrailerURL,CountryProduction,DurationTime,PhotoURL")] MovieFormView movieFormView, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image)
        {
            ViewBag.DirectorID = new SelectList(db.Peoples.Where(p => p.Proffesion.Director == true), "PeopleID", "FullName", movieFormView.DirectorID);
            ViewBag.CountryProduction = new SelectList(CountryList(), movieFormView.CountryProduction);
            ViewBag.DurationTimeValue = movieFormView.DurationTime.ToShortTimeString();

            if (ModelState.IsValid)
            {
                /* DELETED
                if (image != null) {
                    movie.PosterMimeType = image.ContentType;
                    movie.Poster = new byte[image.ContentLength];
                    image.InputStream.Read(movie.Poster, 0, image.ContentLength);
                }
                */
                Movie movie = new Movie {
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

                if (movie.Genre.Count() == 0)
                    movie.Genre = null;

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
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).Single();
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
                TrailerURL = movie.MovieInfo.TrailerURL
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
        public ActionResult Edit(int? id, string RadioPhotoBtn, string UrlPath, HttpPostedFileBase image)
        {
            //[Bind(Include = "MovieID,Title,TitlePL,ReleaseDate,Genre,Description,Poster,PosterMimeType,DirectorID")] Movie movie

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).Single();
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
                TrailerURL = movie.MovieInfo.TrailerURL
            };

            if (TryUpdateModel(movie, "", new string[] { "Title", "TitlePL", "ReleaseDate", "Genre" }))
            {
                if(TryUpdateModel(movie.MovieInfo,"", new string[] { "Description", "DirectorID", "TrailerURL", "CountryProduction", "DurationTime" }))
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
                        ViewData["Error"] = "Nie można zapisać, popraw błędy!";
                        return View(movieFormView);
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
            }
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
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m=>m.Genre).FirstOrDefault(m => m.MovieID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Include(m => m.MovieInfo).Include(m => m.Genre).Where(m => m.MovieID == id).FirstOrDefault();
            db.Movies.Remove(movie);

            IQueryable actorroles = db.ActorRoles.Where(a => a.MovieID == id);
            foreach (ActorRole a in actorroles)
                db.ActorRoles.Remove(a);
            
            db.SaveChanges();
            TempData["Success"] = "Poprawnie usunięto film.";
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

        public static List<string> CountryList()
        {
            List<string> CultureList = new List<string>();

            CultureInfo[] cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultureInfo)
            {
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                if(!(CultureList.Contains(regionInfo.DisplayName)))
                {
                    CultureList.Add(regionInfo.DisplayName);
                }
            }

            CultureList.Sort();
            return CultureList;
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

