using FilmoweJanusze.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FilmoweJanusze.DAL
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("MovieContext")
        {

        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}