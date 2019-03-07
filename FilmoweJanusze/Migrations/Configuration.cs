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
        

        protected override void Seed(FilmoweJanusze.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var movies = new List<Movie>
            {
                new Movie{Title="Titanic",ReleaseDate=DateTime.Parse("1998-02-13"), PhotoURL = "https://cdn.shopify.com/s/files/1/1416/8662/products/titanic_1997_original_film_art_a9fbf094-dad0-487f-bdeb-232cc25b1c6b_2000x.jpg?v=1540065886"},
                new Movie{Title="Aquaman",ReleaseDate=DateTime.Parse("2018-12-19"), PhotoURL = "http://cdn.collider.com/wp-content/uploads/2018/10/aquaman-poster.jpg" },
                new Movie{Title="First Man",TitlePL="Pierwszy cz³owiek",ReleaseDate=DateTime.Parse("2018-10-19"), PhotoURL = "https://i.etsystatic.com/10683147/r/il/aea8ba/1609378563/il_570xN.1609378563_f8r5.jpg" },
                new Movie{Title="Bogowie",ReleaseDate=DateTime.Parse("2014-10-10"), PhotoURL = "https://grybow.pl/wp-content/uploads/2016/04/plakat_bogowie_2.jpg" },
                new Movie{Title="Avengers: Endgame",ReleaseDate=DateTime.Parse("2019-04-25"), PhotoURL = "https://cdn.wegotthiscovered.com/wp-content/uploads/2018/12/Avengers-Endgame-Star-Lord-poster.png" },
                new Movie{Title="The Lion King",TitlePL="Król Lew",ReleaseDate=DateTime.Parse("2019-07-19"), PhotoURL = "https://i.pinimg.com/736x/7e/78/bf/7e78bfc0e0f907f0dc15003e9c537d73.jpg"},
                new Movie{Title="Spider-Man: Far From Home",TitlePL="Spider-Man: Daleko od domu",ReleaseDate=DateTime.Parse("2019-07-05"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/98/69/789869/7876620.6.jpg"},
                new Movie{Title="The Revenant",TitlePL="Zjawa",ReleaseDate=DateTime.Parse("2015-12-16"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/65/83/586583/7722530.3.jpg"},
                new Movie{Title="Inception",TitlePL="Incepcja",ReleaseDate=DateTime.Parse("2010-07-08"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/08/91/500891/7354571.3.jpg"},
                new Movie{Title="The Dark Knight Rises",TitlePL="Mroczny Rycerz powstaje",ReleaseDate=DateTime.Parse("2012-07-16"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/08/91/500891/7354571.3.jpg"},
                new Movie{Title="Interstellar",ReleaseDate=DateTime.Parse("2014-10-26"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/56/29/375629/7670122.5.jpg"},
            };
            movies.ForEach(s => context.Movies.AddOrUpdate(p=>p.Title, s));
            context.SaveChanges();

            var peoples = new List<People>
            {
                new People{FirstName="Leonardo",LastName="DiCaprio",Birthdate=DateTime.Parse("1974-11-11"),PhotoURL="https://ssl-gfx.filmweb.pl/p/00/30/30/398646.1.jpg"},
                new People{FirstName="Kate",LastName="Winslet",Birthdate=DateTime.Parse("1975-10-05"),PhotoURL="https://m.media-amazon.com/images/M/MV5BODgzMzM2NTE0Ml5BMl5BanBnXkFtZTcwMTcyMTkyOQ@@._V1_UX214_CR0,0,214,317_AL_.jpg"},
                new People{FirstName="Jason",LastName="Momoa",Birthdate=DateTime.Parse("1979-08-01"),PhotoURL="https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Jason_Momoa_Supercon_2014.jpg/163px-Jason_Momoa_Supercon_2014.jpg"},
                new People{FirstName="Amber",LastName="Heard",Birthdate=DateTime.Parse("1986-04-22"),PhotoURL="https://ssl-gfx.filmweb.pl/p/95/15/309515/394277.1.jpg"},
                new People{FirstName="Tom",LastName="Holland",Birthdate=DateTime.Parse("1996-06-01"),PhotoURL="https://www.biography.com/.image/t_share/MTQ4MTUwOTQyMDE1OTU2Nzk4/tom-holland-photo-jason-kempin-getty-images-801510482-profile.jpg"},
                new People{FirstName="Robert",LastName="Downey Jr.",Birthdate=DateTime.Parse("1965-04-04"),PhotoURL="https://i.pinimg.com/originals/ba/cd/3c/bacd3c595d372662c502ab3ab945412e.jpg"},
                new People{FirstName="Anne",LastName="Hathaway",Birthdate=DateTime.Parse("1982-11-12"),PhotoURL="https://24smi.org/public/media/celebrity/2018/04/10/j5ilt4q7unyw-anne-hathaway.jpg"},
                new People{FirstName="Christopher",LastName="Nolan",Birthdate=DateTime.Parse("1970-07-30"),PhotoURL="https://i.wpimg.pl/O/341x512/i.wp.pl/a/f/film/033/91/02/0320291.jpg"},
            };
            peoples.ForEach(s => context.Peoples.AddOrUpdate(p => p.FirstName, s));
            context.SaveChanges();



        }
        
    }
}
