using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FilmoweJanusze.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<ActorRole> ActorRoles { get; set; }
        public DbSet<Proffesion> Proffesions { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<UserRate> UserRates { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public ApplicationDbContext()
            : base("MovieContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}