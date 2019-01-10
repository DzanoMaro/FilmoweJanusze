namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRateChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRates", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserRates", "User_Id");
            AddForeignKey("dbo.UserRates", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.UserRates", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRates", "UserID", c => c.String());
            DropForeignKey("dbo.UserRates", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserRates", new[] { "User_Id" });
            DropColumn("dbo.UserRates", "User_Id");
        }
    }
}
