namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleSeperatedTo2Classes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeopleInfoes",
                c => new
                    {
                        PeopleID = c.Int(nullable: false),
                        Birthplace = c.String(),
                        Biography = c.String(),
                        Height = c.Int(),
                    })
                .PrimaryKey(t => t.PeopleID)
                .ForeignKey("dbo.People", t => t.PeopleID)
                .Index(t => t.PeopleID);
            
            DropColumn("dbo.People", "Birthplace");
            DropColumn("dbo.People", "Biography");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Biography", c => c.String());
            AddColumn("dbo.People", "Birthplace", c => c.String());
            DropForeignKey("dbo.PeopleInfoes", "PeopleID", "dbo.People");
            DropIndex("dbo.PeopleInfoes", new[] { "PeopleID" });
            DropTable("dbo.PeopleInfoes");
        }
    }
}
