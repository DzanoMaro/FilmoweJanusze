namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRate_UserId_changeto_UserID : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserRates", new[] { "UserId" });
            CreateIndex("dbo.UserRates", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserRates", new[] { "UserID" });
            CreateIndex("dbo.UserRates", "UserId");
        }
    }
}
