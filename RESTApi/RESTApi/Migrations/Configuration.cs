namespace RESTApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using RESTApi.Models;
    using System.Linq;
    using RESTApi.Utility;

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
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("EASY"), LevelName = "EASY"},
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("MEDIUM"), LevelName = "MEDIUM"},
                new DifficultyLevel { Id = IdGenerator.GenerateIdForDifficultyLevel("HARD"), LevelName = "HARD"}
            );
        }
    }
}
