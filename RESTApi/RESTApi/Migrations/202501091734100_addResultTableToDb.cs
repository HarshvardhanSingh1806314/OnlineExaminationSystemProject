namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResultTableToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Results");
        }
    }
}
