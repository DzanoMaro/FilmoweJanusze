namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleHeightMod : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "Height");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Height", c => c.Int(nullable: false));
        }
    }
}
