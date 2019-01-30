namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoviesDirector : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Movies", "DirectorID");
            AddForeignKey("dbo.Movies", "DirectorID", "dbo.People", "PeopleID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "DirectorID", "dbo.People");
            DropIndex("dbo.Movies", new[] { "DirectorID" });
        }
    }
}
