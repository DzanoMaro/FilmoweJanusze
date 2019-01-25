namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteThumbnailsFromPhoto : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photos", "PhotoThumbURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "PhotoThumbURL", c => c.String());
        }
    }
}
