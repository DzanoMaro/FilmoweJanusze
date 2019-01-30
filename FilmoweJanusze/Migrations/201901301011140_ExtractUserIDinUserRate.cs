namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtractUserIDinUserRate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserRates", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.UserRates", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserRates", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserRates", name: "UserId", newName: "User_Id");
        }
    }
}
