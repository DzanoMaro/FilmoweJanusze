using System;
using System.ComponentModel.DataAnnotations;

namespace FilmoweJanusze.Infrastructure
{
    public class MinDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);

            if (dateTime.Year >= 1900)
                return true;
            else
                return false;
        }
    }
}