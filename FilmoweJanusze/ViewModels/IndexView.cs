using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.ViewModels
{
    public class IndexView
    {
        public virtual IEnumerable<Movie> LatestReleased { get; set; }
        public virtual IEnumerable<Movie> NotYetReleased { get; set; }
        public virtual IEnumerable<People> PeoplesBirthdays { get; set; }
    }
}