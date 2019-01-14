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
                        FirstName = c.String(maxLength: 25),
                        LastName = c.String(maxLength: 25),
                        Birthdate = c.DateTime(nullable: false),
                        PhotoURL = c.String(),
                    })
                .PrimaryKey(t => t.ProfileInfoID);
            
            AddColumn("dbo.AspNetUsers", "ProfileInfo_ProfileInfoID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ProfileInfo_ProfileInfoID");
            AddForeignKey("dbo.AspNetUsers", "ProfileInfo_ProfileInfoID", "dbo.ProfileInfoes", "ProfileInfoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ProfileInfo_ProfileInfoID", "dbo.ProfileInfoes");
            DropIndex("dbo.AspNetUsers", new[] { "ProfileInfo_ProfileInfoID" });
            DropColumn("dbo.AspNetUsers", "ProfileInfo_ProfileInfoID");
            DropTable("dbo.ProfileInfoes");
        }
    }
}
