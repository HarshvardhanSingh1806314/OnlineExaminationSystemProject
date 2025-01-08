using RESTApi.Models;
using System.Data.Entity;

namespace RESTApi.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("name = dbcs")
        {

        }

        //public DbSet<Admin> Admins { get; set; }

        //public DbSet<Test> Tests { get; set; }

        public DbSet<DifficultyLevel> DifficultyLevels { get; set; }
    }
}