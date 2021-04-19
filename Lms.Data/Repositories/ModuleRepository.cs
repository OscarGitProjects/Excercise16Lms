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
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext m_Context;

        public ModuleRepository(ApplicationDbContext context)
        {
            this.m_Context = context;
        }

        public async Task AddAsync<T>(T added)
        {
            await m_Context.AddAsync(added);
        }

        public async Task DeleteAsync(int? id)
        {
            if (!id.HasValue)
                throw new ArgumentException("ModuleRepository. DeleteAsync. Id has no value");

            var module = await GetModuleAsync(id);
            if (module != null)
                m_Context.Module.Remove(module);
        }

        public async Task<IEnumerable<Module>> GetAllModulesBelongingToACourseAsync(int? courseId)
        {
            if (!courseId.HasValue)
                throw new ArgumentException("ModuleRepository. GetAllModulesBelongingToACourseAsync. courseId has no value");

            return await m_Context.Module.Where(c => c.CourseId == courseId).ToListAsync();
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            return await m_Context.Module.ToListAsync();
        }

        public async Task<Module> GetModuleAsync(int? Id)
        {
            if (!Id.HasValue)
                throw new ArgumentException("ModuleRepository. GetModuleAsync. Id has no value");

            return await m_Context.Module.FirstOrDefaultAsync(a => a.Id == Id.Value);
        }

        public async Task<Module> GetModuleAsync(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("ModuleRepository. GetModuleAsync. title is a empty string");

            title = title.Trim();
            title = title.ToLower();

            return await m_Context.Module.FirstOrDefaultAsync(t => t.Title.ToLower() == title);
        }

        public void PutModule(int id, Module module)
        {
            if (module is null)
                throw new ArgumentNullException("ModuleRepository. PutModuleAsync. Reference to course is null");

            m_Context.Entry(module).State = EntityState.Modified;
        }

        public async Task<bool> SaveAsync()
        {
            return (await m_Context.SaveChangesAsync()) >= 0;
        }
    }
}