namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileInfoes",
                c => new
                    {
                        ProfileInfoID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        LastName = c.String(maxLength: 25),
                        Birthdate = c.DateTime(nullable: false),
                        PhotoURL = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProfileInfoID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileInfoes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProfileInfoes", new[] { "User_Id" });
            DropTable("dbo.ProfileInfoes");
        }
    }
}
