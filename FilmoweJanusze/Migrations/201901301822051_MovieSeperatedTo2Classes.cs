namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieSeperatedTo2Classes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "DirectorID", "dbo.People");
            DropIndex("dbo.Movies", new[] { "DirectorID" });
            CreateTable(
                "dbo.MovieInfoes",
                c => new
                    {
                        MovieID = c.Int(nullable: false),
                        CountryProduction = c.String(),
                        DurationTime = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 999),
                        TrailerURL = c.String(),
                        DirectorID = c.Int(),
                        Genre_MovieID = c.Int(),
                    })
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.People", t => t.DirectorID)
                .ForeignKey("dbo.MovieGenres", t => t.Genre_MovieID)
                .ForeignKey("dbo.Movies", t => t.MovieID)
                .Index(t => t.MovieID)
                .Index(t => t.DirectorID)
                .Index(t => t.Genre_MovieID);
            
            DropColumn("dbo.Movies", "CountryProduction");
            DropColumn("dbo.Movies", "DurationTime");
            DropColumn("dbo.Movies", "Description");
            DropColumn("dbo.Movies", "TrailerURL");
            DropColumn("dbo.Movies", "DirectorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "DirectorID", c => c.Int());
            AddColumn("dbo.Movies", "TrailerURL", c => c.String());
            AddColumn("dbo.Movies", "Description", c => c.String(maxLength: 999));
            AddColumn("dbo.Movies", "DurationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movies", "CountryProduction", c => c.String());
            DropForeignKey("dbo.MovieInfoes", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.MovieInfoes", "Genre_MovieID", "dbo.MovieGenres");
            DropForeignKey("dbo.MovieInfoes", "DirectorID", "dbo.People");
            DropIndex("dbo.MovieInfoes", new[] { "Genre_MovieID" });
            DropIndex("dbo.MovieInfoes", new[] { "DirectorID" });
            DropIndex("dbo.MovieInfoes", new[] { "MovieID" });
            DropTable("dbo.MovieInfoes");
            CreateIndex("dbo.Movies", "DirectorID");
            AddForeignKey("dbo.Movies", "DirectorID", "dbo.People", "PeopleID");
        }
    }
}
