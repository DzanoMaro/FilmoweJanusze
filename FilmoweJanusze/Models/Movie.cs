﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using FilmoweJanusze.Infrastructure;

namespace FilmoweJanusze.Models
{
    public class Movie
    {
        //info
        public int MovieID { get; set; }

        [Required(ErrorMessage ="Podaj tytuł filmu")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tytuł filmu musi zawierać od 2 do 100 znaków")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Polski tytuł")]
        public string TitlePL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Podaj datę wydania filmu")]
        [Display(Name = "Data premiery")]
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

        //plakat
        [Display(Name = "Plakat")]
        public byte[] Poster { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string PosterMimeType { get; set; }

        [DataType(DataType.Url)]
        [Display(Name ="Zwiastun")]
        public string TrailerURL { get; set; }

        //reżyser
        [Display(Name = "Reżyser")]
        [DisplayFormat(NullDisplayText = "Brak reżysera")]
        public int? DirectorID { get; set; }

        //obsada
        [Display(Name = "Obsada")]
        public virtual ICollection<ActorRole> Cast { get; set; }
        
        [Display(Name = "Gatunek")]
        [MaximumGenreCount(ErrorMessage ="Możesz wybrać max. 3 kategorie")]
        public virtual MovieGenre Genre { get; set; }

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
        
    }
}