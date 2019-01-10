namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveDubbingFromProffesionToActorRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActorRole", "Dubbing", c => c.Boolean(nullable: false));
            DropColumn("dbo.Proffesion", "Dubbing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proffesion", "Dubbing", c => c.Boolean(nullable: false));
            DropColumn("dbo.ActorRole", "Dubbing");
        }
    }
}
