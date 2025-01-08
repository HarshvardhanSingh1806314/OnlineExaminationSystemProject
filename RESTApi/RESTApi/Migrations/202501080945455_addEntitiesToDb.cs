namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEntitiesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        EmployeeEmail = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        OrganizationName = c.String(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        TotalNoOfEasyQuestions = c.Int(nullable: false),
                        TotalNoOfMediumQuestions = c.Int(nullable: false),
                        TotalNoOfHardQuestions = c.Int(nullable: false),
                        TotalNoOfQuestions = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        AdminId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: true)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(nullable: false, maxLength: 300),
                        Option1 = c.String(nullable: false, maxLength: 200),
                        Option2 = c.String(nullable: false, maxLength: 200),
                        Option3 = c.String(nullable: false, maxLength: 200),
                        Option4 = c.String(nullable: false, maxLength: 200),
                        Answer = c.String(nullable: false, maxLength: 200),
                        DifficultyLevelId = c.String(nullable: false, maxLength: 128),
                        TestId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DifficultyLevels", t => t.DifficultyLevelId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.DifficultyLevelId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        TestId = c.String(nullable: false, maxLength: 128),
                        TotalAttemptsInEasyQuestions = c.Int(nullable: false),
                        CorrectAttempsInEasyQuestions = c.Int(nullable: false),
                        TotalAttemptsInMediumQuestions = c.Int(nullable: false),
                        CorrectAttemptsInMediumQuestions = c.Int(nullable: false),
                        TotalAttemptsInHardQuestions = c.Int(nullable: false),
                        CorrectAttemptsInHardQuestions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        GraduationYear = c.String(nullable: false),
                        City = c.String(),
                        UniversityName = c.String(nullable: false),
                        DegreeMajor = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Reports", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Questions", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Questions", "DifficultyLevelId", "dbo.DifficultyLevels");
            DropForeignKey("dbo.Tests", "AdminId", "dbo.Admins");
            DropIndex("dbo.Reports", new[] { "TestId" });
            DropIndex("dbo.Reports", new[] { "StudentId" });
            DropIndex("dbo.Questions", new[] { "TestId" });
            DropIndex("dbo.Questions", new[] { "DifficultyLevelId" });
            DropIndex("dbo.Tests", new[] { "AdminId" });
            DropTable("dbo.Students");
            DropTable("dbo.Reports");
            DropTable("dbo.Questions");
            DropTable("dbo.Tests");
            DropTable("dbo.Admins");
        }
    }
}
