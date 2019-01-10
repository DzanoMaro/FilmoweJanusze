namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullAbleDirectorID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "DirectorID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movie", "DirectorID", c => c.Int(nullable: false));
        }
    }
}
