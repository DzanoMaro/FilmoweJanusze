namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieDurationTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "DurationTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "DurationTime");
        }
    }
}
