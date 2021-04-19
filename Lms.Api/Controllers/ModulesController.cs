using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork m_Uow;
        private readonly IMapper m_Mapper;

        //public ModulesController(ApplicationDbContext context)
        public ModulesController(IUnitOfWork uow, IMapper mapper)
        {
            //_context = context;
            this.m_Uow = uow;
            this.m_Mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModules()
        {
            List<ModuleDto> lsModules = null;

            //return await _context.Module.ToListAsync();

            var modules = await m_Uow.ModuleRepository.GetAllModulesAsync();
            if (modules != null && modules.Count() > 0)
            {
                ModuleDto dto = null;
                lsModules = new List<ModuleDto>(modules.Count());
                foreach (Module module in modules)
                {
                    dto = m_Mapper.Map<ModuleDto>(module);
                    lsModules.Add(dto);
                }
            }

            return Ok(lsModules);
        }

        // GET: api/Modules/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModuleDto>> GetModuleById(int id)
        {
            ModuleDto dto = null;
            //var @module = await _context.Module.FindAsync(id);
            var module = await m_Uow.ModuleRepository.GetModuleAsync(id);

            if (module == null)
            {
                return NotFound();
            }
            else
            {
                dto = m_Mapper.Map<ModuleDto>(module);
            }

            return Ok(dto);
        }


        [HttpGet("{title}")]
        public async Task<ActionResult<ModuleDto>> GetModule(string title)
        {
            ModuleDto dto = null;
            //var @module = await _context.Module.FindAsync(id);
            var module = await m_Uow.ModuleRepository.GetModuleAsync(title);

            if (module == null)
            {
                return NotFound();
            }
            else
            {
                dto = m_Mapper.Map<ModuleDto>(module);
            }

            return Ok(dto);
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleDto moduleDto)
        {
            if (id != moduleDto.ModuleId)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            Module module = m_Mapper.Map<Module>(moduleDto);

            //_context.Entry(@module).State = EntityState.Modified;
            m_Uow.ModuleRepository.PutModule(id, module);

            try
            {
                //await _context.SaveChangesAsync();
                bool bSaveOk = await m_Uow.ModuleRepository.SaveAsync();

                if (!bSaveOk)
                    return StatusCode(500);
            }
            catch (DbUpdateConcurrencyException)
            {
                var bModuleExists = await ModuleExists(id);
                if (!bModuleExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostModule(ModuleDto moduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //_context.Module.Add(@module);
            //await _context.SaveChangesAsync();
            Module module = m_Mapper.Map<Module>(moduleDto);

            await m_Uow.ModuleRepository.AddAsync(module);
            bool bSaveOk = await m_Uow.ModuleRepository.SaveAsync();

            if (!bSaveOk)
                return StatusCode(500);

            return CreatedAtAction("GetModule", new { id = moduleDto.ModuleId }, moduleDto);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            //var @module = await _context.Module.FindAsync(id);

            var module = await m_Uow.ModuleRepository.GetModuleAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            //_context.Module.Remove(@module);
            //await _context.SaveChangesAsync();

            await m_Uow.ModuleRepository.DeleteAsync(id);
            bool bSaveOk = await m_Uow.ModuleRepository.SaveAsync();

            if (!bSaveOk)
                return StatusCode(500);

            return Ok();
        }

        private async Task<bool> ModuleExists(int id)
        {
            bool bModuleExists = true;

            var module = await m_Uow.ModuleRepository.GetModuleAsync(id);
            if (module is null)
                bModuleExists = false;

                //return _context.Module.Any(e => e.Id == id);

            return bModuleExists;
        }
    }
}
