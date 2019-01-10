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
            indexView.LatestReleased = db.Movies.Where(m => m.ReleaseDate < DateTime.Now).OrderByDescending(m => m.ReleaseDate).Take(3).ToList();
            indexView.NotYetReleased = db.Movies.Where(m => m.ReleaseDate >= DateTime.Now).OrderBy(m => m.ReleaseDate).Take(3).ToList();
            indexView.PeoplesBirthdays = db.Peoples.Where(p => p.Birthdate.Month == DateTime.Now.Month).OrderBy(p => p.Birthdate.Day).ToList();

            return View(indexView);
        }

        public ActionResult Search(string searchString)
        {
            var movies = from m in db.Movies select m;
            var peoples = from p in db.Peoples select p;
            
            if (searchString != null && searchString != "")
            {
                movies = movies.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()) || m.TitlePL.ToUpper().Contains(searchString.ToUpper()));
                peoples = peoples.Where(p => p.FirstName.ToUpper().Contains(searchString.ToUpper()) || p.LastName.ToUpper().Contains(searchString.ToUpper()));

                Found found = new Found();
                found.Movies = movies.ToList();
                found.Peoples = peoples.ToList();
                
                return View(found);
            }
            else
                return RedirectToAction("Index");
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