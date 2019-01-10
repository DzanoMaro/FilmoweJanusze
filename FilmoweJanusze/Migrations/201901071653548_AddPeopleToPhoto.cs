namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeopleToPhoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "MovieID", "dbo.Movies");
            DropIndex("dbo.Photos", new[] { "MovieID" });
            AddColumn("dbo.Photos", "PeopleID", c => c.Int());
            AlterColumn("dbo.Photos", "MovieID", c => c.Int());
            CreateIndex("dbo.Photos", "MovieID");
            CreateIndex("dbo.Photos", "PeopleID");
            AddForeignKey("dbo.Photos", "PeopleID", "dbo.People", "PeopleID");
            AddForeignKey("dbo.Photos", "MovieID", "dbo.Movies", "MovieID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.Photos", "PeopleID", "dbo.People");
            DropIndex("dbo.Photos", new[] { "PeopleID" });
            DropIndex("dbo.Photos", new[] { "MovieID" });
            AlterColumn("dbo.Photos", "MovieID", c => c.Int(nullable: false));
            DropColumn("dbo.Photos", "PeopleID");
            CreateIndex("dbo.Photos", "MovieID");
            AddForeignKey("dbo.Photos", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
        }
    }
}
