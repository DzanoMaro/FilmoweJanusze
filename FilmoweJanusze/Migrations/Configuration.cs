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
                new Movie{Title="Titanic",ReleaseDate=DateTime.Parse("1998-02-13"), PhotoURL = "https://cdn.shopify.com/s/files/1/1416/8662/products/titanic_1997_original_film_art_a9fbf094-dad0-487f-bdeb-232cc25b1c6b_2000x.jpg?v=1540065886", Genre = new Models.MovieGenre{ Drama=true } },
                new Movie{Title="Aquaman",ReleaseDate=DateTime.Parse("2018-12-19"), PhotoURL = "http://cdn.collider.com/wp-content/uploads/2018/10/aquaman-poster.jpg", Genre = new Models.MovieGenre{ Action=true, SciFi=true} },
                new Movie{Title="First Man",TitlePL="Pierwszy cz³owiek",ReleaseDate=DateTime.Parse("2018-10-19"), PhotoURL = "https://i.etsystatic.com/10683147/r/il/aea8ba/1609378563/il_570xN.1609378563_f8r5.jpg", Genre = new Models.MovieGenre{ Drama=true, Biographic=true } },
                new Movie{Title="Bogowie",ReleaseDate=DateTime.Parse("2014-10-10"), PhotoURL = "https://grybow.pl/wp-content/uploads/2016/04/plakat_bogowie_2.jpg", Genre = new Models.MovieGenre{ Drama=true, Biographic=true } },
                new Movie{Title="Avengers: Endgame",ReleaseDate=DateTime.Parse("2019-04-25"), PhotoURL = "https://cdn.wegotthiscovered.com/wp-content/uploads/2018/12/Avengers-Endgame-Star-Lord-poster.png", Genre = new Models.MovieGenre{ Action=true, SciFi=true } },
                new Movie{Title="The Lion King",TitlePL="Król Lew",ReleaseDate=DateTime.Parse("2019-07-19"), PhotoURL = "https://i.pinimg.com/736x/7e/78/bf/7e78bfc0e0f907f0dc15003e9c537d73.jpg", Genre = new Models.MovieGenre{ Anime=true, Comedy=true, Adventure=true }},
                new Movie{Title="Spider-Man: Far From Home",TitlePL="Spider-Man: Daleko od domu",ReleaseDate=DateTime.Parse("2019-07-05"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/98/69/789869/7876620.6.jpg", Genre = new Models.MovieGenre{ Action=true, SciFi=true }},
                new Movie{Title="The Revenant",TitlePL="Zjawa",ReleaseDate=DateTime.Parse("2015-12-16"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/65/83/586583/7722530.3.jpg", Genre = new Models.MovieGenre{ Drama=true, Adventure=true }},
                new Movie{Title="Inception",TitlePL="Incepcja",ReleaseDate=DateTime.Parse("2010-07-08"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/08/91/500891/7354571.3.jpg", Genre = new Models.MovieGenre{ SciFi=true, Thriller=true }},
                new Movie{Title="The Dark Knight Rises",TitlePL="Mroczny Rycerz powstaje",ReleaseDate=DateTime.Parse("2012-07-16"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/08/91/500891/7354571.3.jpg", Genre = new Models.MovieGenre{ Drama=true, Biographic=true }},
                new Movie{Title="Interstellar",ReleaseDate=DateTime.Parse("2014-10-26"), PhotoURL = "https://ssl-gfx.filmweb.pl/po/56/29/375629/7670122.5.jpg", Genre = new Models.MovieGenre{ SciFi=true }},
            };
            movies.ForEach(s => context.Movies.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            var peoples = new List<People>
            {
                new People{FirstName="Leonardo",LastName="DiCaprio",Birthdate=DateTime.Parse("1974-11-11"),PhotoURL="https://ssl-gfx.filmweb.pl/p/00/30/30/398646.1.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Kate",LastName="Winslet",Birthdate=DateTime.Parse("1975-10-05"),PhotoURL="https://m.media-amazon.com/images/M/MV5BODgzMzM2NTE0Ml5BMl5BanBnXkFtZTcwMTcyMTkyOQ@@._V1_UX214_CR0,0,214,317_AL_.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Jason",LastName="Momoa",Birthdate=DateTime.Parse("1979-08-01"),PhotoURL="https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Jason_Momoa_Supercon_2014.jpg/163px-Jason_Momoa_Supercon_2014.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Amber",LastName="Heard",Birthdate=DateTime.Parse("1986-04-22"),PhotoURL="https://ssl-gfx.filmweb.pl/p/95/15/309515/394277.1.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Tom",LastName="Holland",Birthdate=DateTime.Parse("1996-06-01"),PhotoURL="https://www.biography.com/.image/t_share/MTQ4MTUwOTQyMDE1OTU2Nzk4/tom-holland-photo-jason-kempin-getty-images-801510482-profile.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Robert",LastName="Downey Jr.",Birthdate=DateTime.Parse("1965-04-04"),PhotoURL="https://i.pinimg.com/originals/ba/cd/3c/bacd3c595d372662c502ab3ab945412e.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Anne",LastName="Hathaway",Birthdate=DateTime.Parse("1982-11-12"),PhotoURL="https://24smi.org/public/media/celebrity/2018/04/10/j5ilt4q7unyw-anne-hathaway.jpg", Proffesion = new Proffesion{ Actor = true } },
                new People{FirstName="Christopher",LastName="Nolan",Birthdate=DateTime.Parse("1970-07-30"),PhotoURL="https://i.wpimg.pl/O/341x512/i.wp.pl/a/f/film/033/91/02/0320291.jpg", Proffesion = new Proffesion{ Director = true } },
                new People{FirstName="Tom",LastName="Hardy",Birthdate=DateTime.Parse("1977-09-15"),PhotoURL="https://ssl-gfx.filmweb.pl/p/71/04/57104/398789.1.jpg", Proffesion = new Proffesion{ Actor = true } },

            };
            peoples.ForEach(s => context.Peoples.AddOrUpdate(p => p.FirstName, s));
            context.SaveChanges();

            var movieinfoes = new List<MovieInfo>
            {
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Titanic").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("03:14:00"),Description="Rok 1912, brytyjski statek Titanic wyrusza w swój dziewiczy rejs do USA. Na pok³adzie emigrant Jack przypadkowo spotyka arystokratkê Rose.",TrailerURL="https://www.youtube.com/embed/-iRajLSA8TA"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Aquaman").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:23:00"),Description="Arthur Curry niechêtnie staje na czele ludu podwodnego królestwa Atlantydy.",TrailerURL="https://www.youtube.com/embed/6mQDS7Q7pys"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="First Man").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:21:00"),Description="Fragment ¿ycia astronauty Neila Armstronga i jego legendarnej misji kosmicznej, dziêki której jako pierwszy cz³owiek stan¹³ na Ksiê¿ycu.",TrailerURL="https://www.youtube.com/embed/PSoRx87OO6k"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Bogowie").MovieID, CountryProduction="Polska",DurationTime = DateTime.Parse("02:01:00"),Description="Profesor Zbigniew Religa, utalentowany kardiochirurg wierzy, ¿e jest w stanie dokonaæ przeszczepu serca. Nie poddaje siê mimo wielu nieudanych operacji.",TrailerURL="https://www.youtube.com/embed/1biE4cOrDPE"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Avengers: Endgame").MovieID, CountryProduction="Stany Zjednoczone",Description="Po wymazaniu po³owy ¿ycia we Wszechœwiecie przez Thanosa, Avengersi staraj¹ siê zrobiæ wszystko co konieczne, aby pokonaæ szalonego tytana.",TrailerURL="https://www.youtube.com/embed/HiABjI6Y668"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="The Lion King").MovieID, CountryProduction="Stany Zjednoczone",Description="Poœród afrykañskiej sawanny przychodzi na œwiat jej przysz³y król. Ma³y Simba uwielbia swojego ojca, króla Mufasê. Jednak nie wszyscy s¹ szczêœliwi z pojawienia siê m³odego lwi¹tka. Dawny nastêpca tronu, Skaza, ma swoje plany dotycz¹ce przysz³oœci Lwiej ziemi. W efekcie Simba opuszcza swoj¹ rodzinê. ¯yj¹c na wygnaniu musi dorosn¹æ i, z pomoc¹ zaskakuj¹cej dwójki przyjació³, nauczyæ siê jak odebraæ to, co jest mu nale¿ne.",TrailerURL="https://www.youtube.com/embed/4CbLXeGSDxg"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Spider-Man: Far From Home").MovieID, CountryProduction="Stany Zjednoczone",TrailerURL="https://www.youtube.com/embed/1XW1Ygatsz4"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="The Revenant").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:36:00"),Description="Hugh Glass szuka zemsty na ludziach, którzy zostawili go powa¿nie rannego po ataku niedŸwiedzia.",TrailerURL="https://www.youtube.com/embed/7usbQ-VaQKk"},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Inception").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:28:00"),Description="Czasy, gdy technologia pozwala na wchodzenie w œwiat snów. Z³odziej Cobb ma za zadanie wszczepiæ myœl do œpi¹cego umys³u.",TrailerURL="https://www.youtube.com/embed/7usbQ-VaQKk",DirectorID=peoples.Single(p=>p.LastName=="Nolan").PeopleID},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="The Dark Knight Rises").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:45:00"),Description="Po oœmiu latach nieobecnoœci Batman wraca, by uratowaæ Gotham City przed zamaskowanym terroryst¹ Bane'em.",TrailerURL="https://www.youtube.com/embed/GokKUqLcvD8",DirectorID=peoples.Single(p=>p.LastName=="Nolan").PeopleID},
                new MovieInfo{MovieID = movies.Single(m=>m.Title=="Interstellar").MovieID, CountryProduction="Stany Zjednoczone",DurationTime = DateTime.Parse("02:49:00"),Description="Po oœmiu latach nieobecnoœci Batman wraca, by uratowaæ Gotham City przed zamaskowanym terroryst¹ Bane'em.",TrailerURL="https://www.youtube.com/embed/GokKUqLcvD8",DirectorID=peoples.Single(p=>p.LastName=="Nolan").PeopleID},
            };
            movieinfoes.ForEach(s => context.MovieInfos.AddOrUpdate(p => p.MovieID, s));
            context.SaveChanges();

            var peopleinfoes = new List<PeopleInfo>
            {
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Leonardo" && p.LastName=="DiCaprio").PeopleID, Birthplace="Los Angeles, Kalifornia, USA", Height=183},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Kate" && p.LastName=="Winslet").PeopleID, Birthplace="Reading, Anglia, Wielka Brytania", Height=169},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Jason" && p.LastName=="Momoa").PeopleID, Birthplace="Honolulu, Hawaje, USA", Height=193},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Amber" && p.LastName=="Heard").PeopleID, Birthplace="Austin, Teksas, USA", Height=170},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Holland").PeopleID, Birthplace="Kingston upon Thames, Anglia, Wielka Brytania", Height=173},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Robert" && p.LastName=="Downey Jr.").PeopleID, Birthplace="Nowy Jork, Nowy Jork, USA", Height=175},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Anne" && p.LastName=="Hathaway").PeopleID, Birthplace="Nowy Jork, Nowy Jork, USA", Height=173},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Christopher" && p.LastName=="Nolan").PeopleID, Birthplace="Londyn, Anglia, Wielka Brytania", Height=180},
                new PeopleInfo{PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Hardy").PeopleID, Birthplace="Londyn, Anglia, Wielka Brytania", Height=175},

            };
            peopleinfoes.ForEach(s => context.PeopleInfos.AddOrUpdate(p => p.PeopleID, s));
            context.SaveChanges();

            var actorroles = new List<ActorRole>
            {
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Leonardo" && p.LastName=="DiCaprio").PeopleID, MovieID = movies.Single(m=>m.Title == "Titanic").MovieID, RoleName = "Jack Dawson"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Leonardo" && p.LastName=="DiCaprio").PeopleID, MovieID = movies.Single(m=>m.Title == "The Revenant").MovieID, RoleName = "Hugh Glass"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Leonardo" && p.LastName=="DiCaprio").PeopleID, MovieID = movies.Single(m=>m.Title == "Inception").MovieID, RoleName = "Cobb"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Kate" && p.LastName=="Winslet").PeopleID, MovieID = movies.Single(m=>m.Title == "Titanic").MovieID, RoleName = "Rose DeWitt Bukater"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Jason" && p.LastName=="Momoa").PeopleID, MovieID = movies.Single(m=>m.Title == "Aquaman").MovieID, RoleName = "Arthur Curry / Aquaman"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Amber" && p.LastName=="Heard").PeopleID, MovieID = movies.Single(m=>m.Title == "Aquaman").MovieID, RoleName = "Mera"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Holland").PeopleID, MovieID = movies.Single(m=>m.Title == "Spider-Man: Far From Home").MovieID, RoleName = "Peter Parker / Spider-Man"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Robert" && p.LastName=="Downey Jr.").PeopleID, MovieID = movies.Single(m=>m.Title == "Spider-Man: Far From Home").MovieID, RoleName = "Tony Stark / Iron Man"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Robert" && p.LastName=="Downey Jr.").PeopleID, MovieID = movies.Single(m=>m.Title == "Avengers: Endgame").MovieID, RoleName = "Tony Stark / Iron Man"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Holland").PeopleID, MovieID = movies.Single(m=>m.Title == "Avengers: Endgame").MovieID, RoleName = "Peter Parker / Spider-Man"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Hardy").PeopleID, MovieID = movies.Single(m=>m.Title == "The Revenant").MovieID, RoleName = "John Fitzgerald"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Hardy").PeopleID, MovieID = movies.Single(m=>m.Title == "Inception").MovieID, RoleName = "Eames"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Tom" && p.LastName=="Hardy").PeopleID, MovieID = movies.Single(m=>m.Title == "The Dark Knight Rises").MovieID, RoleName = "Bane"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Anne" && p.LastName=="Hathaway").PeopleID, MovieID = movies.Single(m=>m.Title == "Interstellar").MovieID, RoleName = "Brand"},
                new ActorRole{ PeopleID = peoples.Single(p=>p.FirstName=="Anne" && p.LastName=="Hathaway").PeopleID, MovieID = movies.Single(m=>m.Title == "The Dark Knight Rises").MovieID, RoleName = "Selina Kyle / Catwoman"},
            };
            actorroles.ForEach(s => context.ActorRoles.AddOrUpdate(p => p.ActorRoleID, s));
            context.SaveChanges();


        }
        
    }
}
