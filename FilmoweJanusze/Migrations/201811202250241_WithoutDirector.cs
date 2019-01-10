namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithoutDirector : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movie", "Director_PeopleID", "dbo.People");
            DropIndex("dbo.Movie", new[] { "Director_PeopleID" });
            DropColumn("dbo.Movie", "Director_PeopleID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movie", "Director_PeopleID", c => c.Int());
            CreateIndex("dbo.Movie", "Director_PeopleID");
            AddForeignKey("dbo.Movie", "Director_PeopleID", "dbo.People", "PeopleID");
        }
    }
}
