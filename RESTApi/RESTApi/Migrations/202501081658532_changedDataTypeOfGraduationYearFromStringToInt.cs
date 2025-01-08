namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedDataTypeOfGraduationYearFromStringToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "GraduationYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "GraduationYear", c => c.String(nullable: false));
        }
    }
}
