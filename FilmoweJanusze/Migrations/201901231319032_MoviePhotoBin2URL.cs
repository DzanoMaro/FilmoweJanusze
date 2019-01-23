namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoviePhotoBin2URL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "PhotoURL", c => c.String());
            DropColumn("dbo.Movies", "Poster");
            DropColumn("dbo.Movies", "PosterMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "PosterMimeType", c => c.String());
            AddColumn("dbo.Movies", "Poster", c => c.Binary());
            DropColumn("dbo.Movies", "PhotoURL");
        }
    }
}
