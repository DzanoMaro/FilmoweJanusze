namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleInfoUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeopleInfoes", "Height", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeopleInfoes", "Height");
        }
    }
}
