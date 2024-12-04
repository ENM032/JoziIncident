namespace ST10091324_PROG7312_Part1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRequestModel_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequests", "RequestType", c => c.String());
            DropColumn("dbo.ServiceRequests", "IsSearchResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceRequests", "IsSearchResult", c => c.Boolean(nullable: false));
            DropColumn("dbo.ServiceRequests", "RequestType");
        }
    }
}
