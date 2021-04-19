using Lms.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModulesBelongingToACourseAsync(int? courseId);
        Task<IEnumerable<Module>> GetAllModulesAsync();
        Task<Module>GetModuleAsync(int? Id);
        Task<Module> GetModuleAsync(string title);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        Task DeleteAsync(int? id);
        void PutModule(int id, Module module);
    }
}