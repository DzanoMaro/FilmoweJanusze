using System.ComponentModel.DataAnnotations;
using FilmoweJanusze.Models;

namespace FilmoweJanusze.Infrastructure 
{
    public class MaximumGenreCount : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            MovieGenre genre = value as MovieGenre;
            if (genre != null)
            {
                if (genre.Count() > 3)
                    return false;
            }
            return true;
        }
    }
}