namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieAddPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "MovieID", c => c.Int(nullable: false));
            CreateIndex("dbo.Photos", "MovieID");
            AddForeignKey("dbo.Photos", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "MovieID", "dbo.Movies");
            DropIndex("dbo.Photos", new[] { "MovieID" });
            DropColumn("dbo.Photos", "MovieID");
        }
    }
}
