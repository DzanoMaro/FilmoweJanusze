using FilmoweJanusze.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Models
{
    public class MovieInfo
    {
        [Key]
        [ForeignKey("Movie")]
        public int MovieID { get; set; }

        [Display(Name = "Kraj produkcji")]
        [UIHint("CountryProduction")]
        public string CountryProduction { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Czas trwania")]
        [UIHint("DurationTime")]
        public DateTime DurationTime { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "Nie dodano opisu filmu :( ")]
        [StringLength(999, MinimumLength = 25)]
        [Display(Name = "Opis filmu")]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Zwiastun")]
        public string TrailerURL { get; set; }

        //reżyser
        [Display(Name = "Reżyser")]
        [DisplayFormat(NullDisplayText = "Brak reżysera")]
        public int? DirectorID { get; set; }

        [Display(Name = "Gatunek")]
        virtual public MovieGenre Genre { get; set; }

        [ForeignKey("DirectorID")]
        virtual public People Director { get; set; }

        public Movie Movie { get; set; }

                /*
        //plakat
        [Display(Name = "Plakat")]
        public byte[] Poster { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string PosterMimeType { get; set; }
        */
    }
}