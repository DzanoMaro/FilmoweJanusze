namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProfileInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProfileInfoes", "User_Id", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.ProfileInfoes", name: "User_Id", newName: "UserID");
            RenameIndex(table: "dbo.ProfileInfoes", name: "IX_User_Id", newName: "IX_UserID");
            DropPrimaryKey("dbo.ProfileInfoes");
            AddPrimaryKey("dbo.ProfileInfoes", "UserID");
            AddForeignKey("dbo.ProfileInfoes", "UserID", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.ProfileInfoes", "ProfileInfoID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProfileInfoes", "ProfileInfoID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.ProfileInfoes", "UserID", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.ProfileInfoes");
            AddPrimaryKey("dbo.ProfileInfoes", "ProfileInfoID");
            RenameIndex(table: "dbo.ProfileInfoes", name: "IX_UserID", newName: "IX_User_Id");
            RenameColumn(table: "dbo.ProfileInfoes", name: "UserID", newName: "User_Id");
            AddForeignKey("dbo.ProfileInfoes", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
