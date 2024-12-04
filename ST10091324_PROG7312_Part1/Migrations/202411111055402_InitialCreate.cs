namespace ST10091324_PROG7312_Part1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Location = c.String(),
                        Category = c.String(),
                        Description = c.String(),
                        MediaFilePath = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(maxLength: 50),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ProfileImgPath = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocalEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        Category = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ServiceRequests",
                c => new
                    {
                        RequestID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Status = c.String(),
                        Progress = c.Double(nullable: false),
                        IsIndeterminate = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.RequestID)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.LocalEvents", "UserId", "dbo.Users");
            DropForeignKey("dbo.Incidents", "UserId", "dbo.Users");
            DropIndex("dbo.ServiceRequests", new[] { "UserId" });
            DropIndex("dbo.LocalEvents", new[] { "UserId" });
            DropIndex("dbo.Incidents", new[] { "UserId" });
            DropTable("dbo.ServiceRequests");
            DropTable("dbo.LocalEvents");
            DropTable("dbo.Users");
            DropTable("dbo.Incidents");
        }
    }
}
