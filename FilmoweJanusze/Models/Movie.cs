using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using FilmoweJanusze.Infrastructure;
using FilmoweJanusze.ViewModels;

namespace FilmoweJanusze.Models
{
    public class Movie : Tile
    {

        public int MovieID { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        [Display(Name = "Polski tytuł")]
        [StringLength(100, MinimumLength = 2)]
        public string TitlePL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data premiery")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Plakat")]
        [DataType(DataType.Url)]
        override public string PhotoURL { get; set; }

        //ZALEZNOSCI
        public MovieInfo MovieInfo { get; set; }

        [Display(Name = "Gatunek")]
        [Remote("ValidateMovieGenreCount", "Movies", HttpMethod = "POST", ErrorMessage = "Możesz wybrać max. 3 kategorie")]          //po stronie klienta
        [MaximumGenreCount(ErrorMessage = "Możesz wybrać max. 3 kategorie")]                                                       //po stronie serwera
        public MovieGenre Genre { get; set; }

        [Display(Name = "Obsada")]
        public ICollection<ActorRole> Cast { get; set; }
        override public ICollection<UserRate> UserRates { get; set; }
        [Display(Name = "Galeria zdjęć")]
        public ICollection<Photo> Photos { get; set; }


        //METODY
        [Display(Name = "Rok wydania")]
        public int ProductionYear
        {
            get
            {
                return ReleaseDate.Year;
            }
        }

        public string TitleYear
        {
            get
            {
                return Title + " (" + ProductionYear.ToString() + ")";
            }
        }

        //INTERFACE
        override public int ActionID
        {
            get
            {
                return MovieID;
            }
        }

        override public string MainTitle
        {
            get
            {
                return TitleYear;
            }
        }

        override public string SubTitle
        {
            get
            {
                return TitlePL;
            }
        }

        override public string Controller
        {
            get
            {
                return "Movies";
            }
        }
    }
}