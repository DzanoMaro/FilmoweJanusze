namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movedGenreToMovieInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieInfoes", "Genre_MovieID", "dbo.MovieGenres");
            DropIndex("dbo.MovieInfoes", new[] { "Genre_MovieID" });
            DropColumn("dbo.MovieInfoes", "Genre_MovieID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieInfoes", "Genre_MovieID", c => c.Int());
            CreateIndex("dbo.MovieInfoes", "Genre_MovieID");
            AddForeignKey("dbo.MovieInfoes", "Genre_MovieID", "dbo.MovieGenres", "MovieID");
        }
    }
}
