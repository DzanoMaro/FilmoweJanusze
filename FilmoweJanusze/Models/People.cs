using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FilmoweJanusze.Infrastructure;

namespace FilmoweJanusze.Models
{
    public class People : Tile
    {
        public int PeopleID { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        [StringLength(25,MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Zdjęcie")]
        [DataType(DataType.Url)]
        override public string PhotoURL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        public DateTime Birthdate { get; set; }

        //ZALEZNOSCI
        [Display(Name ="Zawód")]
        public Proffesion Proffesion { get; set; }

        public PeopleInfo PeopleInfo { get; set; }

        public ICollection<ActorRole> Roles { get; set; }
        public ICollection<Movie> DirectedMovies { get; set; }
        override public ICollection<UserRate> UserRates { get; set; }
        [Display(Name = "Galeria zdjęć")]
        public ICollection<Photo> Photos { get; set; }

        //METODY
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

        //INTERFACE
        override public int ActionID
        {
            get
            {
                return PeopleID;
            }
        }

        override public string MainTitle
        {
            get
            {
                return FullName;
            }
        }

        override public string SubTitle
        {
            get
            {
                return Age;
            }
        }

        override public string Controller
        {
            get
            {
                return "People";
            }
        }
    }
}