using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    /// <summary>
    /// ModuleRepository med metoder för att spara, uppdatera och radera modules
    /// </summary>
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext m_Context;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="context">Database context</param>
        public ModuleRepository(ApplicationDbContext context)
        {
            this.m_Context = context;
        }


        /// <summary>
        /// Async metod som lägger till Module
        /// </summary>
        /// <typeparam name="T">Module objekt</typeparam>
        /// <param name="added">Module som skall skapas</param>
        /// <returns></returns>
        public async Task AddAsync<T>(T added)
        {
            await m_Context.AddAsync(added);
        }


        /// <summary>
        /// Async metod som raderar en Module
        /// </summary>
        /// <param name="id">id för module som skall raderas</param>
        /// <exception cref="ArgumentException">Kastas om id inte har något värde</exception>
        /// <returns></returns>
        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue)
                throw new ArgumentException("ModuleRepository. DeleteAsync. Id has no value");

            var module = await GetModuleAsync(id);
            if (module != null)
                m_Context.Module.Remove(module);
        }


        /// <summary>
        /// Async metod som returnerar alla modules som tillhör en course
        /// </summary>
        /// <param name="courseId">id för den course som vi söker modules för</param>
        /// <exception cref="ArgumentException">Kastas om courseId inte har något värde</exception>
        /// <returns>IEnumerable med modules som tillhör en course</returns>
        public async Task<IEnumerable<Module>> GetAllModulesBelongingToACourseAsync(int? courseId)
        {
            if (!courseId.HasValue)
                throw new ArgumentException("ModuleRepository. GetAllModulesBelongingToACourseAsync. courseId has no value");

            return await m_Context.Module.Where(c => c.CourseId == courseId).ToListAsync();
        }


        /// <summary>
        /// Async metod som returnerar alla Modules
        /// </summary>
        /// <returns>IEnumerable med modules</returns>
        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            return await m_Context.Module.ToListAsync();
        }


        /// <summary>
        /// Async metod som returnerar sökt module
        /// </summary>
        /// <param name="Id">id för sökt module</param>
        /// <exception cref="ArgumentException">Kastas om Id inte har något värde</exception>
        /// <returns>Sökt modul</returns>
        public async Task<Module> GetModuleAsync(int? Id)
        {
            if (!Id.HasValue)
                throw new ArgumentException("ModuleRepository. GetModuleAsync. Id has no value");

            return await m_Context.Module.FirstOrDefaultAsync(a => a.Id == Id.Value);
        }


        /// <summary>
        /// Async metod som returnerar en model med sökt titel
        /// </summary>
        /// <param name="title">Sökt titel</param>
        /// <exception cref="ArgumentException">Kastas om titel är null eller en tom sträng</exception>
        /// <returns>Sökte model</returns>
        public async Task<Module> GetModuleAsync(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("ModuleRepository. GetModuleAsync. title is a empty string");

            title = title.Trim();
            title = title.ToLower();

            return await m_Context.Module.FirstOrDefaultAsync(t => t.Title.ToLower() == title);
        }


        /// <summary>
        /// Metoden uppdaterar en module
        /// </summary>
        /// <param name="id">id för den module som skall uppdateras</param>
        /// <param name="module">Module med information</param>
        /// <exception cref="ArgumentException">Kastas om referensen till Module är null</exception>
        public void PutModule(int id, Module module)
        {
            if (module is null)
                throw new ArgumentNullException("ModuleRepository. PutModuleAsync. Reference to course is null");

            m_Context.Entry(module).State = EntityState.Modified;
        }


        /// <summary>
        /// Async metod som sparar uppdateringar
        /// </summary>
        /// <returns>true om det sparades några uppdateringar. Annars returneras false</returns>
        public async Task<bool> SaveAsync()
        {
            return (await m_Context.SaveChangesAsync()) >= 0;
        }
    }
}