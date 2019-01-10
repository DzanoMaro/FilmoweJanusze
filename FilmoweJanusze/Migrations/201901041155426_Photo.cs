namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Photo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        PhotoURL = c.String(),
                        ActorRoleID = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("dbo.ActorRoles", t => t.ActorRoleID)
                .Index(t => t.ActorRoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ActorRoleID", "dbo.ActorRoles");
            DropIndex("dbo.Photos", new[] { "ActorRoleID" });
            DropTable("dbo.Photos");
        }
    }
}
