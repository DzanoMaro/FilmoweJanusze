namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Proffesio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proffesion",
                c => new
                    {
                        PeopleID = c.Int(nullable: false),
                        Actor = c.Boolean(nullable: false),
                        Director = c.Boolean(nullable: false),
                        Scenario = c.Boolean(nullable: false),
                        Dubbing = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PeopleID)
                .ForeignKey("dbo.People", t => t.PeopleID)
                .Index(t => t.PeopleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proffesion", "PeopleID", "dbo.People");
            DropIndex("dbo.Proffesion", new[] { "PeopleID" });
            DropTable("dbo.Proffesion");
        }
    }
}
