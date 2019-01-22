using System.ComponentModel.DataAnnotations;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.Infrastructure 
{
    public class MaximumGenreCount : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            MovieGenre movieGenre = value as MovieGenre;
            if (movieGenre != null)
            {
                if (movieGenre.Count() > 3)
                    return false;
            }
            return true;
        }
    }
}