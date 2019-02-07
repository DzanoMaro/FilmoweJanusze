namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeopleInfoRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeopleInfoes", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeopleInfoes", "RowVersion");
        }
    }
}
