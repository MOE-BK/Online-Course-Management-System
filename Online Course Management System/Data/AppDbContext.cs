using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Course_Management_System.Models;

namespace Online_Course_Management_System.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Programming"
                },
                new Category
                {
                    Id = 2,
                    Name = "Database"
                },
                new Category
                {
                    Id = 3,
                    Name = "Design"
                }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = "ASP.NET Core MVC",
                    CategoryId = 1,
                    InstructorId = 1
                },
                new Course
                {
                    Id = 2,
                    Name = "Entity Framework Core",
                    CategoryId = 2,
                    InstructorId = 1
                },
                new Course
                {
                    Id = 3,
                    Name = "UI UX Basics",
                    CategoryId = 3,
                    InstructorId = 2
                }
            );

            modelBuilder.Entity<Instructor>().HasData(
                new Instructor
                {
                    Id = 1,
                    Name = "Ahmed Ali",
                    Email = "ahmed@example.com"
                },
                new Instructor
                {
                    Id = 2,
                    Name = "Mohamed Hassan",
                    Email = "mohamed@example.com"
                }
            );
        }
    }
}