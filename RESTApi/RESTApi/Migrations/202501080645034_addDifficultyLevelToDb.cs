namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDifficultyLevelToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DifficultyLevels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LevelName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DifficultyLevels");
        }
    }
}
