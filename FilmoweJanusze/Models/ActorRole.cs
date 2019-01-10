using System.ComponentModel.DataAnnotations;


namespace FilmoweJanusze.Models
{
    public class ActorRole
    {
        public int ActorRoleID { get; set; }

        [Required]
        [Display(Name = "Aktor")]
        public int PeopleID { get; set; }

        [Required]
        [Display(Name = "Film")]
        public int MovieID { get; set; }

        [Required]
        [Display(Name = "Rola")]
        public string RoleName { get; set; }

        [Display(Name = "Dubbing?")]
        public bool Dubbing { get; set; }

        
        public virtual People People { get; set; }
        public virtual Movie Movie { get; set; }

        public string FullRoleName
        {
            get
            {
                return People.FullName + " jako " + RoleName;
            }
        }

        public string RoleMovie
        {
            get
            {
                return RoleName + " w " + Movie.TitleYear;
            }
        }
    }
}