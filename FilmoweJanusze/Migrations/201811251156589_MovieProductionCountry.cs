namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieProductionCountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "CountryProduction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "CountryProduction");
        }
    }
}
