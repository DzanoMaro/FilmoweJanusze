namespace FilmoweJanusze.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovingContext2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActorRoles",
                c => new
                    {
                        ActorRoleID = c.Int(nullable: false, identity: true),
                        PeopleID = c.Int(nullable: false),
                        MovieID = c.Int(nullable: false),
                        RoleName = c.String(nullable: false),
                        Dubbing = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActorRoleID)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PeopleID, cascadeDelete: true)
                .Index(t => t.PeopleID)
                .Index(t => t.MovieID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        TitlePL = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        CountryProduction = c.String(),
                        DurationTime = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 999),
                        Poster = c.Binary(),
                        PosterMimeType = c.String(),
                        TrailerURL = c.String(),
                        DirectorID = c.Int(),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.MovieGenres",
                c => new
                    {
                        MovieID = c.Int(nullable: false),
                        Action = c.Boolean(nullable: false),
                        Anime = c.Boolean(nullable: false),
                        Biographic = c.Boolean(nullable: false),
                        Documental = c.Boolean(nullable: false),
                        Drama = c.Boolean(nullable: false),
                        Familly = c.Boolean(nullable: false),
                        Fantasy = c.Boolean(nullable: false),
                        Horror = c.Boolean(nullable: false),
                        Comedy = c.Boolean(nullable: false),
                        Short = c.Boolean(nullable: false),
                        Criminal = c.Boolean(nullable: false),
                        Melodrama = c.Boolean(nullable: false),
                        Musical = c.Boolean(nullable: false),
                        Music = c.Boolean(nullable: false),
                        Adventure = c.Boolean(nullable: false),
                        Romans = c.Boolean(nullable: false),
                        SciFi = c.Boolean(nullable: false),
                        Thriller = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.Movies", t => t.MovieID)
                .Index(t => t.MovieID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PeopleID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        LastName = c.String(nullable: false, maxLength: 25),
                        Birthdate = c.DateTime(nullable: false),
                        Birthplace = c.String(),
                        Biography = c.String(),
                        FacePhoto = c.Binary(),
                        FaceMimeType = c.String(),
                    })
                .PrimaryKey(t => t.PeopleID);
            
            CreateTable(
                "dbo.Proffesions",
                c => new
                    {
                        PeopleID = c.Int(nullable: false),
                        Actor = c.Boolean(nullable: false),
                        Director = c.Boolean(nullable: false),
                        Scenario = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PeopleID)
                .ForeignKey("dbo.People", t => t.PeopleID)
                .Index(t => t.PeopleID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRates",
                c => new
                    {
                        UserRateID = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        UserID = c.String(),
                        Rate = c.Int(nullable: false),
                        Comment = c.String(maxLength: 99),
                    })
                .PrimaryKey(t => t.UserRateID)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.MovieID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRates", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ActorRoles", "PeopleID", "dbo.People");
            DropForeignKey("dbo.Proffesions", "PeopleID", "dbo.People");
            DropForeignKey("dbo.MovieGenres", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.ActorRoles", "MovieID", "dbo.Movies");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserRates", new[] { "MovieID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Proffesions", new[] { "PeopleID" });
            DropIndex("dbo.MovieGenres", new[] { "MovieID" });
            DropIndex("dbo.ActorRoles", new[] { "MovieID" });
            DropIndex("dbo.ActorRoles", new[] { "PeopleID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserRates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Proffesions");
            DropTable("dbo.People");
            DropTable("dbo.MovieGenres");
            DropTable("dbo.Movies");
            DropTable("dbo.ActorRoles");
        }
    }
}
