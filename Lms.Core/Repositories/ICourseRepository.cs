using Lms.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules = false);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseAsync(int? Id);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        Task DeleteAsync(int? id);
        void PutCourse(int id, Course course);
    }
}