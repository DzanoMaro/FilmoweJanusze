namespace FilmoweJanusze.Migrations
{
    using FilmoweJanusze.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FilmoweJanusze.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        /*
        protected override void Seed()
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var movies = new List<Movie>
            {
                //new Movie{Title="Titanic",ReleaseDate=DateTime.Now.Date, Genre="Dramat", Description="Andrzeje dwa co siê rucha³y na statku, Andrzeje dwa co siê rucha³y na statku, Andrzeje dwa co siê rucha³y na statku"}
            };

            //movies.ForEach(s => context.Movies.Add(s));
            //context.SaveChanges();
        }
        */
    }
}
