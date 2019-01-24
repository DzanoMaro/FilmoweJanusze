using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.ViewModels
{
    public class ProfileInfoesDetails
    {
        public ProfileInfo ProfileInfo { get; set; }

        [Display(Name = "Ocenione filmy")]
        public ICollection<UserRate> RatedMovies { get; set; }
        [Display(Name = "Ocenioni ludzie filmu")]
        public ICollection<UserRate> RatedPeoples { get; set; }

        [Display(Name = "Utworzone filmy")]
        public ICollection<Movie> CreatedMovies { get; set; }
        [Display(Name = "Ostatnio edytowane filmy")]
        public ICollection<Movie> LastEditedMovies { get; set; }

        [Display(Name = "Utworzeni ludzie kina")]
        public ICollection<People> CreatedPeoples { get; set; }
        [Display(Name = "Ostatnio edytowani ludzie kina")]
        public ICollection<People> LastEditedPeoples { get; set; }
    }
}