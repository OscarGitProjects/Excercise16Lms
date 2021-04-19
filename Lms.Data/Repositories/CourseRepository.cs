using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext m_Context;

        public CourseRepository(ApplicationDbContext context)
        {
            this.m_Context = context;
        }
        public async Task AddAsync<T>(T added)
        {
            await m_Context.AddAsync(added);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules = false)
        {
            var courses = includeModules == true ? await m_Context.Course.Include(m => m.Modules).ToListAsync()
                : await m_Context.Course.ToListAsync();

            return courses;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await m_Context.Course.Include(m => m.Modules).ToListAsync();
        }

        public async Task<Course> GetCourseAsync(int? Id)
        {
            if (!Id.HasValue)
                throw new ArgumentException("CourseRepository. GetCourse. Id has no value");

            return await m_Context.Course.Include(m => m.Modules).FirstOrDefaultAsync(a => a.CourseId == Id.Value);
        }

        public async Task<bool> SaveAsync()
        {
            return (await m_Context.SaveChangesAsync()) >= 0;
        }

        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue)
                throw new ArgumentException("CourseRepository. DeleteAsync. Id has no value");

            var course = await GetCourseAsync(id);
            if (course != null)
                m_Context.Course.Remove(course);

            return;
        }


        public void PutCourse(int id, Course course)
        {
            if (course is null)
                throw new ArgumentNullException("CourseRepository. PutCourseAsync. Reference to course is null");

            m_Context.Entry(course).State = EntityState.Modified;
        }
    }
}
