namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewPeopleData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Birthplace", c => c.String());
            AddColumn("dbo.People", "Height", c => c.Int(nullable: false));
            AddColumn("dbo.People", "Biography", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Biography");
            DropColumn("dbo.People", "Height");
            DropColumn("dbo.People", "Birthplace");
        }
    }
}
