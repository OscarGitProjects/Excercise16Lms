using Lms.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    /// <summary>
    /// Interface för CourseRepository
    /// </summary>
    public interface ICourseRepository
    {
        /// <summary>
        /// Async metod som returnerar alla courses. Om includeModules = true returneras också modules som tillhör en course
        /// </summary>
        /// <param name="includeModules">true om en course även skall ha tillhörande modules. Annars false. Default false</param>
        /// <returns>IEnumerable med courses</returns>
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules = false);

        /// <summary>
        /// Async metod som returnerar alla courses
        /// </summary>
        /// <returns>IEnumerable med alla courses</returns>
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        /// <summary>
        /// Async metod som returnera en sökt course
        /// </summary>
        /// <param name="Id">id för course som söks</param>
        /// <exception cref="ArgumentException">Kastas om Id inte har något värde</exception>
        /// <returns>Sökt course</returns>
        Task<Course> GetCourseAsync(int? Id);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Async metod som lägger till Course objekt
        /// </summary>
        /// <typeparam name="T">Course objekt</typeparam>
        /// <param name="added">Course som skall skapas</param>
        /// <returns></returns>
        Task AddAsync<T>(T added);

        /// <summary>
        /// Async metod som raderar en course
        /// </summary>
        /// <param name="id">id för den course som skall raderas</param>
        /// <exception cref="ArgumentException">Kastas om id inte har något värde</exception>
        /// <returns></returns>
        Task DeleteAsync(int? id);

        /// <summary>
        /// Metoden uppdaterar informationen om en course
        /// </summary>
        /// <param name="id">id för den course som skall uppdateras</param>
        /// <param name="course">Course med data</param>
        /// <exception cref="ArgumentException">Kastas om referensen till course objektet är null</exception>
        void PutCourse(int id, Course course);
    }
}