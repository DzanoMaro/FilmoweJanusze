namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActorRole",
                c => new
                    {
                        ActorRoleID = c.Int(nullable: false, identity: true),
                        PeopleID = c.Int(nullable: false),
                        MovieID = c.Int(nullable: false),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActorRoleID)
                .ForeignKey("dbo.Movie", t => t.MovieID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PeopleID, cascadeDelete: true)
                .Index(t => t.PeopleID)
                .Index(t => t.MovieID);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        TitlePL = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Genre = c.String(nullable: false),
                        Description = c.String(maxLength: 999),
                        Poster = c.Binary(),
                        PosterMimeType = c.String(),
                        DirectorID = c.Int(nullable: false),
                        Director_PeopleID = c.Int(),
                    })
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.People", t => t.Director_PeopleID)
                .Index(t => t.Director_PeopleID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PeopleID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        LastName = c.String(nullable: false, maxLength: 25),
                        Birthdate = c.DateTime(nullable: false),
                        FacePhoto = c.Binary(),
                        FaceMimeType = c.String(),
                    })
                .PrimaryKey(t => t.PeopleID);
            
        }
        
        public override void Down()
        {
            
            DropForeignKey("dbo.Movie", "Director_PeopleID", "dbo.People");
            DropForeignKey("dbo.ActorRole", "PeopleID", "dbo.People");
            DropForeignKey("dbo.ActorRole", "MovieID", "dbo.Movie");
            DropIndex("dbo.Movie", new[] { "Director_PeopleID" });
            DropIndex("dbo.ActorRole", new[] { "MovieID" });
            DropIndex("dbo.ActorRole", new[] { "PeopleID" });
            DropTable("dbo.People");
            DropTable("dbo.Movie");
            DropTable("dbo.ActorRole");
            
        }
    }
}
