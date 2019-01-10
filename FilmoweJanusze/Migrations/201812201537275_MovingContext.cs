namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovingContext : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActorRole", "MovieID", "dbo.Movie");
            DropForeignKey("dbo.MovieGenre", "MovieID", "dbo.Movie");
            DropForeignKey("dbo.Proffesion", "PeopleID", "dbo.People");
            DropForeignKey("dbo.ActorRole", "PeopleID", "dbo.People");
            DropForeignKey("dbo.UserRate", "MovieID", "dbo.Movie");
            DropIndex("dbo.ActorRole", new[] { "PeopleID" });
            DropIndex("dbo.ActorRole", new[] { "MovieID" });
            DropIndex("dbo.MovieGenre", new[] { "MovieID" });
            DropIndex("dbo.Proffesion", new[] { "PeopleID" });
            DropIndex("dbo.UserRate", new[] { "MovieID" });
            DropTable("dbo.ActorRole");
            DropTable("dbo.Movie");
            DropTable("dbo.MovieGenre");
            DropTable("dbo.People");
            DropTable("dbo.Proffesion");
            DropTable("dbo.UserRate");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserRate",
                c => new
                    {
                        UserRateID = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        UserID = c.String(),
                        Rate = c.Int(nullable: false),
                        Comment = c.String(maxLength: 99),
                    })
                .PrimaryKey(t => t.UserRateID);
            
            CreateTable(
                "dbo.Proffesion",
                c => new
                    {
                        PeopleID = c.Int(nullable: false),
                        Actor = c.Boolean(nullable: false),
                        Director = c.Boolean(nullable: false),
                        Scenario = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PeopleID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PeopleID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        LastName = c.String(nullable: false, maxLength: 25),
                        Birthdate = c.DateTime(nullable: false),
                        Birthplace = c.String(),
                        Biography = c.String(),
                        FacePhoto = c.Binary(),
                        FaceMimeType = c.String(),
                    })
                .PrimaryKey(t => t.PeopleID);
            
            CreateTable(
                "dbo.MovieGenre",
                c => new
                    {
                        MovieID = c.Int(nullable: false),
                        Action = c.Boolean(nullable: false),
                        Anime = c.Boolean(nullable: false),
                        Biographic = c.Boolean(nullable: false),
                        Documental = c.Boolean(nullable: false),
                        Drama = c.Boolean(nullable: false),
                        Familly = c.Boolean(nullable: false),
                        Fantasy = c.Boolean(nullable: false),
                        Horror = c.Boolean(nullable: false),
                        Comedy = c.Boolean(nullable: false),
                        Short = c.Boolean(nullable: false),
                        Criminal = c.Boolean(nullable: false),
                        Melodrama = c.Boolean(nullable: false),
                        Musical = c.Boolean(nullable: false),
                        Music = c.Boolean(nullable: false),
                        Adventure = c.Boolean(nullable: false),
                        Romans = c.Boolean(nullable: false),
                        SciFi = c.Boolean(nullable: false),
                        Thriller = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        TitlePL = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        CountryProduction = c.String(),
                        DurationTime = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 999),
                        Poster = c.Binary(),
                        PosterMimeType = c.String(),
                        TrailerURL = c.String(),
                        DirectorID = c.Int(),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.ActorRole",
                c => new
                    {
                        ActorRoleID = c.Int(nullable: false, identity: true),
                        PeopleID = c.Int(nullable: false),
                        MovieID = c.Int(nullable: false),
                        RoleName = c.String(nullable: false),
                        Dubbing = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActorRoleID);
            
            CreateIndex("dbo.UserRate", "MovieID");
            CreateIndex("dbo.Proffesion", "PeopleID");
            CreateIndex("dbo.MovieGenre", "MovieID");
            CreateIndex("dbo.ActorRole", "MovieID");
            CreateIndex("dbo.ActorRole", "PeopleID");
            AddForeignKey("dbo.UserRate", "MovieID", "dbo.Movie", "MovieID", cascadeDelete: true);
            AddForeignKey("dbo.ActorRole", "PeopleID", "dbo.People", "PeopleID", cascadeDelete: true);
            AddForeignKey("dbo.Proffesion", "PeopleID", "dbo.People", "PeopleID");
            AddForeignKey("dbo.MovieGenre", "MovieID", "dbo.Movie", "MovieID");
            AddForeignKey("dbo.ActorRole", "MovieID", "dbo.Movie", "MovieID", cascadeDelete: true);
        }
    }
}
