namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoThumbURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "PhotoThumbURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "PhotoThumbURL");
        }
    }
}
