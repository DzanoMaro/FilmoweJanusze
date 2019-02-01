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
    public class PeopleFormView
    {

        [Required(ErrorMessage = "Podaj imię")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} musi zawierać od 2 do 25 znaków")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko")]
        [Display(Name = "Nazwisko")]
        [StringLength(25, ErrorMessage = "{0} musi zawierać od  do  znaków", MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        public string LastName { get; set; }

        [Display(Name = "Zdjęcie")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }

        [Display(Name = "Zawód")]
        [MinimumProffesionCount(ErrorMessage = "Wybierz przynajmniej jeden zawód")]
        public Proffesion Proffesion { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        [CheckBirthday(ErrorMessage = "{0} nie może być z przyszłości")]
        [MinDate(ErrorMessage = "Data nie może być sprzed 1900r.")]
        [Remote("CheckBirthdate", "Extended", ErrorMessage = "{0} nie może być z przyszłości, ani sprzed 1900r.")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Miejsce urodzenia")]
        public string Birthplace { get; set; }

        [Display(Name = "Wzrost")]
        [Range(50, 220, ErrorMessage = "{0} musi mieścić się w granicy {1} - {2} cm")]
        [DisplayFormat(DataFormatString = "{0} cm")]
        public int? Height { get; set; }

        [Display(Name = "Biografia")]
        [DisplayFormat(NullDisplayText = "Nie dodano biografii :( ")]
        [StringLength(9999, ErrorMessage = "Nie możesz przekroczyć 9999 znaków")]
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }
    }
}