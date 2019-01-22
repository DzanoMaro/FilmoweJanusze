using FilmoweJanusze.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Models
{
    public class ProfileInfo
    {
        public int ProfileInfoID { get; set; }

        [Required(ErrorMessage = "Podaj swoje imię")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} musi zawierać od {2} do {1} znaków")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} musi zawierać od {2} do {1} znaków")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        [CheckBirthday(ErrorMessage = "{0} nie może być z przyszłości")]
        [Remote("CheckBirthdate", "People", ErrorMessage = "{0} nie może być z przyszłości")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Zdjęcie")]
        [DisplayFormat(NullDisplayText = "~\\Images\\Brak_zdjecia_usera.png")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}