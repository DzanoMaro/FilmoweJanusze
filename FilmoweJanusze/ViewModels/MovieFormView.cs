using FilmoweJanusze.Infrastructure;
using FilmoweJanusze.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.ViewModels
{
    public class MovieFormView
    {
        public int MovieID { get; set; }

        [Required(ErrorMessage = "Podaj tytuł filmu")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} filmu musi zawierać od 2 do 100 znaków")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Polski tytuł")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} filmu musi zawierać od 2 do 100 znaków")]
        public string TitlePL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Podaj datę wydania filmu")]
        [Display(Name = "Data premiery")]
        [MinDate(ErrorMessage = "{0} nie może być sprzed 1900r.")]
        [Remote("CheckMinReleaseDate", "Extended", ErrorMessage = "{0} nie może być z przyszłości, ani sprzed 1900r.")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Kraj produkcji")]
        [UIHint("CountryProduction")]
        public string CountryProduction { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Czas trwania")]
        [UIHint("DurationTime")]
        public DateTime DurationTime { get; set; }

        [StringLength(999, MinimumLength = 25, ErrorMessage = "Opis filmu musi zawierać od 25 do 999 znaków")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "Nie dodano opisu filmu :( ")]
        [Display(Name = "Opis filmu")]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Zwiastun")]
        public string TrailerURL { get; set; }

        [Display(Name = "Plakat")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }

        //reżyser
        [Display(Name = "Reżyser")]
        [DisplayFormat(NullDisplayText = "Brak reżysera")]
        public int? DirectorID { get; set; }

        [Display(Name = "Gatunek")]
        [Remote("ValidateMovieGenreCount", "Movies", HttpMethod = "POST", ErrorMessage = "Możesz wybrać max. 3 kategorie")]          //po stronie klienta
        [MaximumGenreCount(ErrorMessage = "Możesz wybrać max. 3 kategorie")]                                                       //po stronie serwera
        virtual public MovieGenre Genre { get; set; }
    }
}