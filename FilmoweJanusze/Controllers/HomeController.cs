using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmoweJanusze.Models;
using FilmoweJanusze.ViewModels;
using FilmoweJanusze.DAL;

namespace FilmoweJanusze.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            IndexView indexView = new IndexView();
            indexView.LatestReleased = db.Movies.Where(m => m.ReleaseDate < DateTime.Now).OrderByDescending(m => m.ReleaseDate).ToList();
            indexView.NotYetReleased = db.Movies.Where(m => m.ReleaseDate >= DateTime.Now).OrderBy(m => m.ReleaseDate).ToList();
            indexView.PeoplesBirthdays = db.Peoples.Where(p => p.Birthdate.Month == DateTime.Now.Month).OrderBy(p => p.Birthdate.Day).ToList();

            return View(indexView);
        }

        public ActionResult Search(string searchString)
        {
            if (searchString != null && searchString != "")
            {
                if(searchString.Contains("movieid="))
                {
                    var movieid = int.Parse(searchString.Substring(8));
                    return RedirectToAction("Details", "Movies", new { id = movieid });
                }

                if (searchString.Contains("peopleid="))
                {
                    var peopleid = int.Parse(searchString.Substring(9));
                    return RedirectToAction("Details", "People", new { id = peopleid });
                }

                Found found = new Found();
                found.Movies = db.Movies.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()) || m.TitlePL.ToUpper().Contains(searchString.ToUpper())).ToList();
                found.Peoples = db.Peoples.Where(p => p.FirstName.ToUpper().Contains(searchString.ToUpper()) || p.LastName.ToUpper().Contains(searchString.ToUpper()) || searchString.ToUpper() == p.FirstName.ToUpper() + " " + p.LastName.ToUpper()).ToList();
                
                return View(found);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult SearchAutoComplete(string searchString)
        {
            // Found found = new Found();
           var movielist = db.Movies.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()) || m.TitlePL.ToUpper().Contains(searchString.ToUpper())).Take(3).Select(m => new { label = m.Title + " (" + m.ReleaseDate.Year + ")", val = "movieid=" + m.MovieID}).ToList();
           var peoplelist = db.Peoples.Where(p => p.FirstName.ToUpper().Contains(searchString.ToUpper()) || p.LastName.ToUpper().Contains(searchString.ToUpper()) || searchString.ToUpper() == p.FirstName.ToUpper() + " " + p.LastName.ToUpper()).Take(3).Select(p => new { label = p.FirstName + " " + p.LastName, val = "peopleid=" + p.PeopleID}).ToList();

           return Json(movielist.Concat(peoplelist).ToList(), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}