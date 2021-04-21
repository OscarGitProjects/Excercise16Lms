using Lms.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    /// <summary>
    /// Interface för ModuleRepository
    /// </summary>
    public interface IModuleRepository
    {
        /// <summary>
        /// Async metod som returnerar alla modules som tillhör en course
        /// </summary>
        /// <param name="courseId">id för den course som vi söker modules för</param>
        /// <exception cref="ArgumentException">Kastas om courseId inte har något värde</exception>
        /// <returns>IEnumerable med modules som tillhör en course</returns>
        Task<IEnumerable<Module>> GetAllModulesBelongingToACourseAsync(int? courseId);

        /// <summary>
        /// Async metod som returnerar alla Modules
        /// </summary>
        /// <returns>IEnumerable med modules</returns>
        Task<IEnumerable<Module>> GetAllModulesAsync();

        /// <summary>
        /// Async metod som returnerar sökt module
        /// </summary>
        /// <param name="Id">id för sökt module</param>
        /// <exception cref="ArgumentException">Kastas om Id inte har något värde</exception>
        /// <returns>Sökt modul</returns>
        Task<Module>GetModuleAsync(int? Id);

        /// <summary>
        /// Async metod som returnerar en model med sökt titel
        /// </summary>
        /// <param name="title">Sökt titel</param>
        /// <exception cref="ArgumentException">Kastas om titel är null eller en tom sträng</exception>
        /// <returns>Sökte model</returns>
        Task<Module> GetModuleAsync(string title);

        /// <summary>
        /// Async metod som sparar uppdateringar
        /// </summary>
        /// <returns>true om det sparades några uppdateringar. Annars returneras false</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Async metod som lägger till Module
        /// </summary>
        /// <typeparam name="T">Module objekt</typeparam>
        /// <param name="added">Module som skall skapas</param>
        /// <returns></returns>
        Task AddAsync<T>(T added);

        /// <summary>
        /// Async metod som raderar en Module
        /// </summary>
        /// <param name="id">id för module som skall raderas</param>
        /// <exception cref="ArgumentException">Kastas om id inte har något värde</exception>
        /// <returns></returns>
        Task DeleteAsync(int? id);

        /// <summary>
        /// Metoden uppdaterar en module
        /// </summary>
        /// <param name="id">id för den module som skall uppdateras</param>
        /// <param name="module">Module med information</param>
        /// <exception cref="ArgumentException">Kastas om referensen till Module är null</exception>
        void PutModule(int id, Module module);
    }
}