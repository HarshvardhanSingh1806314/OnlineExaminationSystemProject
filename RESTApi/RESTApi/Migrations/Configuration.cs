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
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("EASY"), LevelName = "EASY" },
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("MEDIUM"), LevelName = "MEDIUM" },
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("HARD"), LevelName = "HARD" }
            );

            // seeding Roles Table with Roles
            context.Roles.AddOrUpdate(
                r => r.RoleId,
                new Role { RoleId = IdGenerator.GenerateIdForRole("STUDENT"), Name = "STUDENT" },
                new Role { RoleId = IdGenerator.GenerateIdForRole("ADMIN"), Name = "ADMIN" }
            );
        }
    }
}
