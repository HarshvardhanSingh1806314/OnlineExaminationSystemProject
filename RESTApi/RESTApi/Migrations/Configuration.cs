namespace RESTApi.Migrations
{
    using RESTApi.Models;
    using RESTApi.Utility;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RESTApi.DataAccess.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RESTApi.DataAccess.ApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // seeding DiffucltyLevel Table with difficulty levels
            context.DifficultyLevels.AddOrUpdate(
                dl => dl.Id,
                new DifficultyLevel { 
                    Id = IdGenerator.GenerateIdForDifficultyLevel(StaticDetails.DIFFICULTY_EASY),
                    LevelName = StaticDetails.DIFFICULTY_EASY 
                },
                new DifficultyLevel { 
                    Id = IdGenerator.GenerateIdForDifficultyLevel(StaticDetails.DIFFICULTY_MEDIUM), 
                    LevelName = StaticDetails.DIFFICULTY_MEDIUM 
                },
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel(StaticDetails.DIFFICULTY_HARD), 
                    LevelName = StaticDetails.DIFFICULTY_HARD 
                }
            );

            //// seeding Roles Table with Roles
            context.Roles.AddOrUpdate(
                r => r.RoleId,
                new Role { RoleId = IdGenerator.GenerateIdForRole(StaticDetails.ROLE_STUDENT), Name = StaticDetails.ROLE_STUDENT },
                new Role { RoleId = IdGenerator.GenerateIdForRole(StaticDetails.ROLE_ADMIN), Name = StaticDetails.ROLE_ADMIN }
            );

            // seeding Result Table with Results
            context.Results.AddOrUpdate(
                r => r.ResultId,
                new Result { ResultId = IdGenerator.GenerateIdForResults(StaticDetails.RESULT_PASSED), Name = StaticDetails.RESULT_PASSED },
                new Result { ResultId = IdGenerator.GenerateIdForResults(StaticDetails.RESULT_FAILED), Name = StaticDetails.RESULT_FAILED }
            );
        }
    }
}
