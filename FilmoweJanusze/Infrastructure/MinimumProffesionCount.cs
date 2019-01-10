using System.ComponentModel.DataAnnotations;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.Infrastructure
{
    public class MinimumProffesionCount : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Proffesion proffesion = value as Proffesion;
            bool checkOK = false;
            if (proffesion != null)
            {
                if (proffesion.Actor == true || proffesion.Director == true || proffesion.Scenario == true)
                    checkOK = true;
            }
            return checkOK;
        }
    }
}