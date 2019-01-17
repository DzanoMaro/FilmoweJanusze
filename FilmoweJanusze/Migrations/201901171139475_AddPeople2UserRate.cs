namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeople2UserRate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRates", "MovieID", "dbo.Movies");
            DropIndex("dbo.UserRates", new[] { "MovieID" });
            AddColumn("dbo.UserRates", "PeopleID", c => c.Int());
            AlterColumn("dbo.UserRates", "MovieID", c => c.Int());
            CreateIndex("dbo.UserRates", "MovieID");
            CreateIndex("dbo.UserRates", "PeopleID");
            AddForeignKey("dbo.UserRates", "PeopleID", "dbo.People", "PeopleID");
            AddForeignKey("dbo.UserRates", "MovieID", "dbo.Movies", "MovieID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRates", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.UserRates", "PeopleID", "dbo.People");
            DropIndex("dbo.UserRates", new[] { "PeopleID" });
            DropIndex("dbo.UserRates", new[] { "MovieID" });
            AlterColumn("dbo.UserRates", "MovieID", c => c.Int(nullable: false));
            DropColumn("dbo.UserRates", "PeopleID");
            CreateIndex("dbo.UserRates", "MovieID");
            AddForeignKey("dbo.UserRates", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
        }
    }
}
