using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    /// <summary>
    /// CourseRepository med metoder för att spara, uppdatera och radera courses
    /// </summary>
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext m_Context;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context">Database context</param>
        public CourseRepository(ApplicationDbContext context)
        {
            this.m_Context = context;
        }


        /// <summary>
        /// Async metod som lägger till Course objekt
        /// </summary>
        /// <typeparam name="T">Course objekt</typeparam>
        /// <param name="added">Course som skall skapas</param>
        /// <returns></returns>
        public async Task AddAsync<T>(T added)
        {
            await m_Context.AddAsync(added);
        }


        /// <summary>
        /// Async metod som returnerar alla courses. Om includeModules = true returneras också modules som tillhör en course
        /// </summary>
        /// <param name="includeModules">true om en course även skall ha tillhörande modules. Annars false. Default false</param>
        /// <returns>IEnumerable med courses</returns>
        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules = false)
        {
            var courses = includeModules == true ? await m_Context.Course.Include(m => m.Modules).ToListAsync()
                : await m_Context.Course.ToListAsync();

            return courses;
        }

        /// <summary>
        /// Async metod som returnerar alla courses
        /// </summary>
        /// <returns>IEnumerable med alla courses</returns>
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await m_Context.Course.Include(m => m.Modules).ToListAsync();
        }


        /// <summary>
        /// Async metod som returnera en sökt course
        /// </summary>
        /// <param name="Id">id för course som söks</param>
        /// <exception cref="ArgumentException">Kastas om Id inte har något värde</exception>
        /// <returns>Sökt course</returns>
        public async Task<Course> GetCourseAsync(int? Id)
        {
            if (!Id.HasValue)
                throw new ArgumentException("CourseRepository. GetCourse. Id has no value");

            return await m_Context.Course.Include(m => m.Modules).FirstOrDefaultAsync(a => a.CourseId == Id.Value);
        }


        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        public async Task<bool> SaveAsync()
        {
            return (await m_Context.SaveChangesAsync()) >= 0;
        }


        /// <summary>
        /// Async metod som raderar en course
        /// </summary>
        /// <param name="id">id för den course som skall raderas</param>
        /// <exception cref="ArgumentException">Kastas om id inte har något värde</exception>
        /// <returns></returns>
        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue)
                throw new ArgumentException("CourseRepository. DeleteAsync. Id has no value");

            var course = await GetCourseAsync(id);
            if (course != null)
                m_Context.Course.Remove(course);

            return;
        }

        /// <summary>
        /// Metoden uppdaterar informationen om en course
        /// </summary>
        /// <param name="id">id för den course som skall uppdateras</param>
        /// <param name="course">Course med data</param>
        /// <exception cref="ArgumentException">Kastas om referensen till course objektet är null</exception>
        public void PutCourse(int id, Course course)
        {
            if (course is null)
                throw new ArgumentNullException("CourseRepository. PutCourseAsync. Reference to course is null");

            m_Context.Entry(course).State = EntityState.Modified;
        }
    }
}
