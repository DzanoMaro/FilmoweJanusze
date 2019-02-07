using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class Proffesion
    {
        [Key]
        [ForeignKey("People")]
        public int PeopleID { get; set; }

        [Display(Name = "Aktor")]
        public bool Actor { get; set; }
        [Display(Name = "Reżyser")]
        public bool Director { get; set; }
        [Display(Name = "Scenarzysta")]
        public bool Scenario { get; set; }


        public People People { get; set; }

        static public string[] GetProffesions()
        {
            return new[] { "Aktor", "Reżyser", "Scenarzysta" };
        }

        public bool EqualTo(Proffesion proffesion)
        {
            if (
                this.Actor == proffesion.Actor &&
                this.Director == proffesion.Director &&
                this.Scenario == proffesion.Scenario
                )
                return true;
            else
                return false;
        }
    }
}