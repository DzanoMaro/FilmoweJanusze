namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnumGenre : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "Genre", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movie", "Genre", c => c.String(nullable: false));
        }
    }
}
