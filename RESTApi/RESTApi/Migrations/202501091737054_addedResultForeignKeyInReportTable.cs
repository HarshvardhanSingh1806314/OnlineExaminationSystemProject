namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedResultForeignKeyInReportTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "ResultId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reports", "ResultId");
            AddForeignKey("dbo.Reports", "ResultId", "dbo.Results", "ResultId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "ResultId", "dbo.Results");
            DropIndex("dbo.Reports", new[] { "ResultId" });
            DropColumn("dbo.Reports", "ResultId");
        }
    }
}
