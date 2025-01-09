using RESTApi.Models;
using System.Data.Entity;

namespace RESTApi.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("name = dbcs")
        {

        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<DifficultyLevel> DifficultyLevels { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // adding foreign key constraint and adding on cascading properties

            // Configure the primary key Id to not be an identity column
            modelBuilder.Entity<Admin>()
                .Property(a => a.AdminId)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // defining foreign key relationship between Report and Student
            modelBuilder.Entity<Report>()
                .HasRequired(s => s.Student)
                .WithMany(r => r.Reports)
                .HasForeignKey(s => s.StudentId)
                .WillCascadeOnDelete(true);

            // defining foreign key relationship between Report and Test
            modelBuilder.Entity<Report>()
                .HasRequired(t => t.Test)
                .WithMany(r => r.Reports)
                .HasForeignKey(t => t.TestId)
                .WillCascadeOnDelete(true);

            // defining foreign key relationship between Admin and Test
            modelBuilder.Entity<Test>()
                .HasRequired(a => a.Admin)
                .WithMany(t => t.Tests)
                .HasForeignKey(a => a.AdminId)
                .WillCascadeOnDelete(true);

            // defining foreign key relationship between Question and Test
            modelBuilder.Entity<Question>()
                .HasRequired(t => t.Test)
                .WithMany(q => q.Questions)
                .HasForeignKey(t => t.TestId)
                .WillCascadeOnDelete(true);
        }
    }
}