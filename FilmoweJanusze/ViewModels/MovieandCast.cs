using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.ViewModels
{
    public class MovieandCast
    {
        public Movie Movie { get; set; }
        public People People { get; set; }
        public UserRate LoggedInURate { get; set; }
        public double AvgRate { get; set; }

        [Display(Name = "Galeria zdjęć")]
        public ICollection<Photo> Photos { get; set; }
        //public ICollection<ActorRole> Cast { get; set; }
        //public ICollection<Movie> DirectedMovies { get; set; }
        public ICollection<UserRate> UserRates { get; set; }
    }
}