namespace ST10091324_PROG7312_Part1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRequestModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequests", "IsSearchResult", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceRequests", "IsSearchResult");
        }
    }
}
