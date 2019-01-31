namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitTitlePLinMovie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "TitlePL", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "TitlePL", c => c.String());
        }
    }
}
