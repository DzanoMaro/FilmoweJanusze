using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FilmoweJanusze.Infrastructure;

namespace FilmoweJanusze.Models
{
    public class People
    {
        public int PeopleID { get; set; }

        [Required(ErrorMessage ="Podaj imię")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} musi zawierać od 2 do 25 znaków")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$",ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko")]
        [Display(Name = "Nazwisko")]
        [StringLength(25, ErrorMessage = "{0} musi zawierać od  do  znaków", MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "{0} nie może zawierać znaków specjalnych, ani liczb")]
        public string LastName { get; set; }

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
        [Range(0,220,ErrorMessage = "{0} musi być nieujemny i nie może przekraczać 220cm")]
        [DisplayFormat(DataFormatString = "{0} cm")]
        public uint? Height { get; set; }

        [Display(Name = "Biografia")]
        [DisplayFormat(NullDisplayText = "Nie dodano biografii :( ")]
        [StringLength(9999,ErrorMessage ="Nie możesz przekroczyć 9999 znaków")]
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }

        /*
        [Display(Name = "Zdjęcie")]
        public byte[] FacePhoto { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string FaceMimeType { get; set; }
        */

        [Display(Name = "Zdjęcie")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }

        }

        [Display(Name = "Wiek")]
        public string Age
        {
            get
            {
                int age = DateTime.Now.Year - Birthdate.Year;

                if (DateTime.Now.Month < Birthdate.Month || (DateTime.Now.Month == Birthdate.Month && DateTime.Now.Day < Birthdate.Day))
                    age--;

                if (age > 0)
                {
                    return age.ToString() + " lat";
                }
                else
                {
                    return "Noworodek";
                }
            }
        }

        [Display(Name ="Zawód")]
        [MinimumProffesionCount(ErrorMessage = "Wybierz przynajmniej jeden zawód")]
        public Proffesion Proffesion { get; set; }
        public ICollection<ActorRole> Roles { get; set; }
        public ICollection<Movie> DirectedMovies { get; set; }

    }
}