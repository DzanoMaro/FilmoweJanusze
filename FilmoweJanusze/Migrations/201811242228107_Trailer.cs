namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trailer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "TrailerURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "TrailerURL");
        }
    }
}
