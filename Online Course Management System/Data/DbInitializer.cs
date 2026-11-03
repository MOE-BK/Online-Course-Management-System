using Microsoft.EntityFrameworkCore;
using Online_Course_Management_System.Data;
using Online_Course_Management_System.Models;

namespace Online_Course_Management_System.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Name = "Programming" },
                    new Category { Name = "Database" },
                    new Category { Name = "Design" }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Instructors.AnyAsync())
            {
                context.Instructors.AddRange(
                    new Instructor
                    {
                        Name = "Ahmed Ali",
                        Email = "ahmed@example.com"
                    },
                    new Instructor
                    {
                        Name = "Mohamed Hassan",
                        Email = "mohamed@example.com"
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Courses.AnyAsync())
            {
                var programming = await context.Categories
                    .FirstAsync(c => c.Name == "Programming");

                var database = await context.Categories
                    .FirstAsync(c => c.Name == "Database");

                var ahmed = await context.Instructors
                    .FirstAsync(i => i.Name == "Ahmed Ali");

                context.Courses.AddRange(
                    new Course
                    {
                        Name = "ASP.NET Core MVC",
                        Description = "Learn ASP.NET Core MVC.",
                        Price = 1500,
                        Duration = 40,
                        CreatedAt = DateTime.Now,
                        CategoryId = programming.Id,
                        InstructorId = ahmed.Id
                    },
                    new Course
                    {
                        Name = "Entity Framework Core",
                        Description = "Learn EF Core.",
                        Price = 1200,
                        Duration = 30,
                        CreatedAt = DateTime.Now,
                        CategoryId = database.Id,
                        InstructorId = ahmed.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}