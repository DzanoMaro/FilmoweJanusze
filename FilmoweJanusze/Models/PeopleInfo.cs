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
    public class PeopleInfo
    {
        [Key]
        [ForeignKey("People")]
        public int PeopleID { get; set; }

        [Display(Name = "Miejsce urodzenia")]
        public string Birthplace { get; set; }

        [Display(Name = "Wzrost")]
        [Range(50, 220)]
        [DisplayFormat(DataFormatString = "{0} cm")]
        public int? Height { get; set; }

        [Display(Name = "Biografia")]
        [DisplayFormat(NullDisplayText = "Nie dodano biografii :( ")]
        [StringLength(9999)]
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }


        /*
        [Display(Name = "Zdjęcie")]
        public byte[] FacePhoto { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string FaceMimeType { get; set; }
        */

        //ZALEZNOSCI
        public People People { get; set; }

        //METODY


    }
}