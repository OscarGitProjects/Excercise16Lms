using Bogus;
using Lms.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitSeedAsync(IServiceProvider services)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var fake = new Faker("sv");

                int iNumberOfAddedCourses = 0;

                // Skapa kurs 1
                var course1 = new Course
                {
                    Title = $"{iNumberOfAddedCourses} Course Title",
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses)                
                };

                context.Course.Add(course1);

                await context.SaveChangesAsync();

                context.Module.Add(new Module { 
                    CourseId = course1.CourseId,
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses),
                    Title = $"{1} Module Title. Course: {course1.CourseId}",
                });

                context.Module.Add(new Module
                {
                    CourseId = course1.CourseId,
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses + 1),
                    Title = $"{2} Module Title. Course: {course1.CourseId}",
                });

                iNumberOfAddedCourses++;
                await context.SaveChangesAsync();


                // Skapa kurs 2
                var course2 = new Course
                {
                    Title = $"{iNumberOfAddedCourses} Course Title",
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses)
                };

                context.Course.Add(course2);

                await context.SaveChangesAsync();

                context.Module.Add(new Module
                {
                    CourseId = course2.CourseId,
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses),
                    Title = $"{1} Module Title. Course: {course2.CourseId}",
                });

                context.Module.Add(new Module
                {
                    CourseId = course2.CourseId,
                    StartDate = DateTime.Now.AddDays(iNumberOfAddedCourses + 1),
                    Title = $"{2} Module Title. Course: {course2.CourseId}",
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
