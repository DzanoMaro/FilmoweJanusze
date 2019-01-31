using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmoweJanusze.Infrastructure
{
    interface ITile
    {
        int ActionID { get;}
        string Controller { get;}


        string MainTitle { get;}
        string SubTitle { get;}
        string PhotoURL { get; set; }

    }
}
