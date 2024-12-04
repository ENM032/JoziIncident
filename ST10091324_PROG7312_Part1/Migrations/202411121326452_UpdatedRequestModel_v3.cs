namespace ST10091324_PROG7312_Part1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRequestModel_v3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocalEvents", "UserId", "dbo.Users");
            DropIndex("dbo.LocalEvents", new[] { "UserId" });
            AddColumn("dbo.ServiceRequests", "SearchRequestType", c => c.String());
            DropColumn("dbo.LocalEvents", "UserId");
            DropColumn("dbo.ServiceRequests", "RequestType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceRequests", "RequestType", c => c.String());
            AddColumn("dbo.LocalEvents", "UserId", c => c.Guid(nullable: false));
            DropColumn("dbo.ServiceRequests", "SearchRequestType");
            CreateIndex("dbo.LocalEvents", "UserId");
            AddForeignKey("dbo.LocalEvents", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
