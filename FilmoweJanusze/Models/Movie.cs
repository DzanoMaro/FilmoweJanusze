using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using FilmoweJanusze.Infrastructure;
using FilmoweJanusze.ViewModels;

namespace FilmoweJanusze.Models
{
    public class Movie : ITile
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
        public string PhotoURL { get; set; }

        //obsada
        [Display(Name = "Obsada")]
        public ICollection<ActorRole> Cast { get; set; }

        public MovieInfo MovieInfo { get; set; }

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
        public int ActionID
        {
            get
            {
                return MovieID;
            }
        }

        public string MainTitle
        {
            get
            {
                return TitleYear;
            }
        }

        public string SubTitle
        {
            get
            {
                return TitlePL;
            }
        }

        public string Controller
        {
            get
            {
                return "Movies";
            }
        }
    }
}