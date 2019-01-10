using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.ViewModels
{
    public class Found
    {
        public virtual IEnumerable<Movie> Movies { get; set; }
        public virtual IEnumerable<People> Peoples { get; set; }
    }
}