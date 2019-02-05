using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.Controllers
{
    public class ExtendedController : Controller
    {

        //REKORDY
        protected string NoContentPhoto = "/Images/Brak_zdjecia.png";
        protected string NoUserPhoto = "/Images/Brak_zdjecia_usera.png";
        protected int DefPageSize = 8;
        protected int DefPageNo = 1;
        protected int TilesPerCarousel = 4;

        //WALIDACJA JSON
        public JsonResult CheckBirthdate(string Birthdate)
        {
            DateTime dateTime = new DateTime();
            if (DateTime.TryParse(Birthdate, out dateTime))
            {
                if (dateTime <= DateTime.Today && dateTime.Year >= 1900)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Nieprawidłowy format daty", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckMinReleaseDate(string ReleaseDate)
        {
            DateTime dateTime = new DateTime();
            if( DateTime.TryParse(ReleaseDate, out dateTime) )
            {
                if (dateTime.Year >= 1900)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Nieprawidłowy format daty", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckImageSelected(string RadioPhotoBtn)
        {
            if (RadioPhotoBtn == "FromFile")
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //METODY UŻYTKOWE
        public HttpStatusCodeResult CheckID(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
        public HttpStatusCodeResult CheckID(string id)
        {
            if (String.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        public List<string> CountryList()
        {
            List<string> CultureList = new List<string>();

            CultureInfo[] cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultureInfo)
            {
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                if (!(CultureList.Contains(regionInfo.DisplayName)))
                {
                    CultureList.Add(regionInfo.DisplayName);
                }
            }

            CultureList.Sort();
            return CultureList;
        }
        public int[] PageSizes()
        {
            return new[] { 4, 8, 12, 20 };
        }
        public string[] SortOrder()
        {
            return new[] { "Rosnąco", "Malejąco" };
        }

        public IEnumerable<Movie> FilterRate(IEnumerable<Movie> tiles, string rate, string UserID)
        {
            int irate;
            if (int.TryParse(rate, out irate))
            {
                return tiles.Where(t => t.UserRates.Any(ur => ur.Rate == irate && ur.UserID == UserID));
            }
            else
                return tiles;
            
        }
        public IEnumerable<People> FilterRate(IEnumerable<People> tiles, string rate, string UserID)
        {
            int irate;
            if (int.TryParse(rate, out irate))
            {
                return tiles.Where(t => t.UserRates.Any(ur => ur.Rate == irate && ur.UserID == UserID));
            }
            else
                return tiles;
        }

        public IEnumerable<Movie> SwitchGenre(IEnumerable<Movie> movies, string genre)
        {
            IEnumerable<Movie> genremovies = movies;
            switch (genre)
            {
                case "Akcja":
                    genremovies = genremovies.Where(m => m.Genre.Action == true);
                    break;
                case "Animowany":
                    genremovies = genremovies.Where(m => m.Genre.Anime == true);
                    break;
                case "Biograficzny":
                    genremovies = genremovies.Where(m => m.Genre.Biographic == true);
                    break;
                case "Dokumentalny":
                    genremovies = genremovies.Where(m => m.Genre.Documental == true);
                    break;
                case "Dramat":
                    genremovies = genremovies.Where(m => m.Genre.Drama == true);
                    break;
                case "Familijny":
                    genremovies = genremovies.Where(m => m.Genre.Familly == true);
                    break;
                case "Fantasy":
                    genremovies = genremovies.Where(m => m.Genre.Fantasy == true);
                    break;
                case "Horror":
                    genremovies = genremovies.Where(m => m.Genre.Horror == true);
                    break;
                case "Komedia":
                    genremovies = genremovies.Where(m => m.Genre.Comedy == true);
                    break;
                case "Krótkometrażowy":
                    genremovies = genremovies.Where(m => m.Genre.Short == true);
                    break;
                case "Krotkometrażowy":    //dont delete
                    genremovies = genremovies.Where(m => m.Genre.Short == true);
                    genre = "Krótkometrażowy";
                    break;
                case "Kryminalny":
                    genremovies = genremovies.Where(m => m.Genre.Criminal == true);
                    break;
                case "Melodramat":
                    genremovies = genremovies.Where(m => m.Genre.Melodrama == true);
                    break;
                case "Musical":
                    genremovies = genremovies.Where(m => m.Genre.Musical == true);
                    break;
                case "Muzyczny":
                    genremovies = genremovies.Where(m => m.Genre.Music == true);
                    break;
                case "Przygodowy":
                    genremovies = genremovies.Where(m => m.Genre.Adventure == true);
                    break;
                case "Romans":
                    genremovies = genremovies.Where(m => m.Genre.Romans == true);
                    break;
                case "Sci-Fi":
                    genremovies = genremovies.Where(m => m.Genre.SciFi == true);
                    break;
                case "Thriller":
                    genremovies = genremovies.Where(m => m.Genre.Thriller == true);
                    break;
                default:
                    break;
            }
            return genremovies;
        }
        public IEnumerable<Movie> FilterCountry(IEnumerable<Movie> movies, string countryProduction)
        {
            if (!String.IsNullOrEmpty(countryProduction))
            {
                movies = movies.Where(m => m.MovieInfo.CountryProduction == countryProduction);
            }
            return movies;
        }
        public IEnumerable<Movie> FilterYears(IEnumerable<Movie> movies, string years)
        {
            if (!String.IsNullOrEmpty(years))
            {
                movies = movies.Where(m => m.ReleaseDate.Year.ToString().Equals(years));
            }
            return movies;
        }
        public IEnumerable<Movie> SortMovies(IEnumerable<Movie> movies, string sortParam, string sortOrder)
        {
            if (sortParam == "Daty premiery")
            {
                if (sortOrder == "Malejąco")
                    movies = movies.OrderByDescending(m => m.ReleaseDate);
                else
                    movies = movies.OrderBy(m => m.ReleaseDate);
            }
            else if (sortParam == "Oceny")
            {
                var nonrated = movies.Where(m=>m.UserRates.Count()==0);
                var rated = movies.Where(m=>m.UserRates.Count()>0);

                if (sortOrder == "Malejąco")
                { 
                    rated = rated.OrderByDescending(m => m.UserRates.Average(ur => ur.Rate)).ToList();
                    movies = rated.Union(nonrated);
                }
                else
                { 
                    rated = rated.OrderBy(m => m.UserRates.Average(ur => ur.Rate)).ToList();
                    movies = nonrated.Union(rated);
                }
            }
            else
            {
                if (sortOrder == "Malejąco")
                    movies = movies.OrderByDescending(m => m.Title);
                else
                    movies = movies.OrderBy(m => m.Title);
            }
            return movies;
        }

        public IEnumerable<People> SwitchProffesion(IEnumerable<People> peoples, string proffesion)
        {
            switch (proffesion)
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
            return peoples;
        }
        public IEnumerable<People> SortPeoples(IEnumerable<People> peoples, string sortParam, string sortOrder)
        {
            switch (sortParam)
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
                case "Oceny":
                    {
                        var nonrated = peoples.Where(p => p.UserRates.Count() == 0);
                        var rated = peoples.Where(p => p.UserRates.Count() > 0);

                        if (sortOrder == "Malejąco")
                        {
                            rated = rated.OrderByDescending(p => p.UserRates.Average(ur => ur.Rate));
                            peoples = rated.Union(nonrated);
                        }
                        else
                        {
                            rated = rated.OrderBy(p => p.UserRates.Average(ur => ur.Rate));
                            peoples = nonrated.Union(rated);
                        }
                    }
                    break;
                default:
                    peoples = peoples.OrderBy(p => p.LastName);
                    break;
            }
            return peoples;
        }


        public bool SaveCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            //source:https://www.codeproject.com/Tips/683699/%2FTips%2F683699%2FCropping-an-Image-from-Center-i

            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //g.CompositingQuality = CompositingQuality.HighQuality;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }

        public string SaveNewFile(string foldername, string filename, HttpPostedFileBase image)
        {
            return SaveNewFile(foldername, filename, image, true);
        }
        public string SaveNewFile(string foldername, string filename, HttpPostedFileBase image, bool overwite)
        {
            string root = Server.MapPath("~/");
            string folder = "/Images/" + foldername + "/";
            string name = filename;
            string ext = System.IO.Path.GetExtension(image.FileName);

            //tworzy folder
            System.IO.Directory.CreateDirectory(root + folder);

            if (System.IO.File.Exists(root + folder + name + ext) && overwite == false)
            {
                int i = 0;
                do
                {
                    i++;
                    name = filename + i.ToString();
                }
                while (System.IO.File.Exists(root + folder + name + ext));
            }

            //zapisanie zdjecia wg sciezki do konkretnego miejsca na dysku
            image.SaveAs(root + folder + name + ext);

            return folder + name + ext;
        }

        public void DeleteOldFile(string Url)
        {
            if(Url != NoContentPhoto)
                if (System.IO.File.Exists(Server.MapPath("~/") + Url))
                    System.IO.File.Delete(Server.MapPath("~/") + Url);
        }

        public string IfEmptySetEmptyPhoto(string Url)
        {
            return IfEmptySetEmptyPhoto(Url, NoContentPhoto);
        }
        public string IfEmptySetEmptyPhoto(string Url, string defPhotUrl)
        {
            if (String.IsNullOrEmpty(Url))
                return defPhotUrl;
            else return Url;
        }

    }
}