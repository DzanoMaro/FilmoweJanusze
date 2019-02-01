using FilmoweJanusze.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FilmoweJanusze.Models
{
    abstract public class Tile : ITile
    {
        abstract public int ActionID { get; }

        abstract public string Controller { get;}

        abstract public string MainTitle { get; }

        abstract public string SubTitle { get; }

        abstract public string PhotoURL { get; set; }
        abstract public ICollection<UserRate> UserRates { get; set; }
    }
}