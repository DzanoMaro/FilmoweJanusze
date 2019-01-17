using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class UserRate
    {
        public int UserRateID { get; set; }

        public int? MovieID { get; set; }
        public int? PeopleID { get; set; }

        [Display(Name = "Twoja ocena")]
        [Range(1, 6)]
        public int Rate { get; set; }

        [StringLength(99, ErrorMessage ="Komentarz może zawierać maksymalnie 99 znaków")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "Brak komentarza")]
        [Display(Name = "Twój komentarz")]
        public string Comment { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual People People { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}