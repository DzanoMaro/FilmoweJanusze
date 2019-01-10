using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class Photo
    {

        public int PhotoID { get; set; }
        [Display(Name = "Zdjęcie")]
        [DataType(DataType.Url)]
        public string PhotoURL { get; set; }
        public string PhotoThumbURL { get; set; }

        public int? MovieID { get; set; }
        public int? PeopleID { get; set; }

        [Display(Name = "Rola filmowa ze zdjęcia")]
        public int? ActorRoleID { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual People People { get; set; }
        public virtual ActorRole ActorRole { get; set; }

        public string Description
        {
            get
            {
                if (ActorRole != null)
                {
                    return ActorRole.FullRoleName;
                }
                else if (MovieID != null)
                {
                    return Movie.TitleYear;
                }
                else if (PeopleID != null)
                {
                    return People.FullName;
                }
                else
                    return "";
            }
        }
    }
}