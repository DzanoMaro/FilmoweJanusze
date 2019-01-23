namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeoplePhotoBin2URL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "PhotoURL", c => c.String());
            DropColumn("dbo.People", "FacePhoto");
            DropColumn("dbo.People", "FaceMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "FaceMimeType", c => c.String());
            AddColumn("dbo.People", "FacePhoto", c => c.Binary());
            DropColumn("dbo.People", "PhotoURL");
        }
    }
}
