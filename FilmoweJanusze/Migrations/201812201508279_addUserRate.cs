namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserRate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRate",
                c => new
                    {
                        UserRateID = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        UserID = c.String(),
                        Rate = c.Int(nullable: false),
                        Comment = c.String(maxLength: 99),
                    })
                .PrimaryKey(t => t.UserRateID)
                .ForeignKey("dbo.Movie", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.MovieID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRate", "MovieID", "dbo.Movie");
            DropIndex("dbo.UserRate", new[] { "MovieID" });
            DropTable("dbo.UserRate");
        }
    }
}
