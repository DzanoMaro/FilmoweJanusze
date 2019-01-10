namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieGenre : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.Movie", t => t.MovieID)
                .Index(t => t.MovieID);
            
            DropColumn("dbo.Movie", "Genre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movie", "Genre", c => c.Int(nullable: false));
            DropForeignKey("dbo.MovieGenre", "MovieID", "dbo.Movie");
            DropIndex("dbo.MovieGenre", new[] { "MovieID" });
            DropTable("dbo.MovieGenre");
        }
    }
}
