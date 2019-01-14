using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class ProfileInfo
    {
        public int ProfileInfoID { get; set; }

        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} musi zawierać od 2 do 25 znaków")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(25, ErrorMessage = "{0} musi zawierać od  do  znaków", MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Zdjęcie")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }
    }
}