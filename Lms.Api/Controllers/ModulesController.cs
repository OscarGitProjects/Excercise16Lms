using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Api.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/modules")]
    public class ModulesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork m_Uow;
        private readonly IMapper m_Mapper;

        //public ModulesController(ApplicationDbContext context)

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        public ModulesController(IUnitOfWork uow, IMapper mapper)
        {
            //_context = context;
            this.m_Uow = uow;
            this.m_Mapper = mapper;
        }


        /// <summary>
        /// GET: api/Modules
        /// Get alla modules
        /// </summary>
        /// <returns>Ok = 200 och en lista med modules</returns>
        /// <response code="200">Returnerade lista med modules</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
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


        /// <summary>
        /// GET: api/Modules/5
        /// Get sökt module
        /// </summary>
        /// <param name="id">id för sökt module</param>
        /// <returns>Ok = 200 och sökt module eller NotFound = 404</returns>
        /// <response code="200">Returnerade sökt module</response>
        /// <response code="404">Sökt module finns inte</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// GET: api/Modules/title
        /// Get sökt module på titel
        /// </summary>
        /// <param name="title">titel för sökt module</param>
        /// <returns>Ok = 200 och sökt module eller NotFound = 404</returns>
        /// <response code="200">Returnerar sökt module</response>
        /// <response code="404">Sökt module finns inte</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// 
        /// PUT: api/Modules/5
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// Put, update/replace, module
        /// </summary>
        /// <param name="id">id för den module som skall uppdateras</param>
        /// <param name="moduleDto">Information om den module som skall uppdateras</param>
        /// <returns>Om det gick bra returneras Ok = 200. 
        /// Om id och moduledto ej har samma id returneras BadRequest = 400. 
        /// Om ModelState ej är valid returneras BadRequest = 400.
        /// Om module som skall uppdateras inte finns returneras NotFound = 404.
        /// Om det inte gick uppdatera module returneras StatusCode = 500.
        /// </returns>
        /// <response code="200">Uppdatering av module gick bra</response>
        /// <response code="400">Id och module id är inte samma eller ModelState isn't valid</response>
        /// <response code="404">Module som skall uppdateras finns ej</response>
        /// <response code="500">Det gick inte uppdatera informationen om Module</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary> 
        /// POST: api/Modules
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// Post, create, ny module
        /// </summary>
        /// <param name="moduleDto">Information om den module som skall skapas</param>
        /// <returns>Om det gick bra sker det en redirect till GetModule action.
        /// Om ModelState ej är valid returneras BadRequest = 400.
        /// Om det inte gick att skapa module returneras StatusCode = 500
        /// </returns>
        /// <response code="400">ModelState isn't valid</response>
        /// <response code="500">Det gick inte skapa en ny model</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// DELETE: api/Modules/5
        /// DELETE, radera, en module
        /// </summary>
        /// <param name="id">id för den module som skall raderas</param>
        /// <returns>Om det gick radera module returneras Ok = 200.
        /// Om module inte finns returneras NotFound = 404.
        /// Om det inte gick att radera module returneras StatusCode = 500.
        /// </returns>
        /// <response code="200">Raderade module</response>
        /// <response code="404">Module finns inte</response>
        /// <response code="500">Det gick inte radera module</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// Metoden testar om en module finns
        /// </summary>
        /// <param name="id">id för sökt module</param>
        /// <returns>true om sökt module finns. Annars returneras false</returns>
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
