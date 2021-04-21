using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Models.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        // private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork m_Uow;
        private readonly IMapper m_Mapper;

        //public CoursesController(ApplicationDbContext context, IUnitOfWork uow)

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        public CoursesController(IUnitOfWork uow, IMapper mapper)
        {
            //_context = context;
            this.m_Uow = uow;
            this.m_Mapper = mapper;
        }

        // GET: api/Courses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        //{
        //    List<CourseDto> lsCourses = null;

        //    // return await _context.Course.ToListAsync();
        //    var courses = await m_Uow.CourseRepository.GetAllCoursesAsync();
        //    if (courses != null && courses.Count() > 0)
        //    {
        //        CourseDto dto = null;
        //        lsCourses = new List<CourseDto>(courses.Count());
        //        foreach (Course course in courses)
        //        {
        //            dto = m_Mapper.Map<CourseDto>(course);
        //            lsCourses.Add(dto);
        //        }
        //    }

        //    return Ok(lsCourses);
        //}

        /// <summary>
        /// Get alla courses
        /// </summary>
        /// <param name="includeModules">true om courses även skall innehålla modules. Annars false. Default false</param>
        /// <returns>Ok = 200 och en lista med courses</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses([FromQuery]bool includeModules = false)
        {
            List<CourseDto> lsCourses = null;

            // return await _context.Course.ToListAsync();
            var courses = await m_Uow.CourseRepository.GetAllCoursesAsync(includeModules);
            if (courses != null && courses.Count() > 0)
            {
                CourseDto dto = null;
                lsCourses = new List<CourseDto>(courses.Count());
                foreach (Course course in courses)
                {
                    dto = m_Mapper.Map<CourseDto>(course);
                    lsCourses.Add(dto);
                }
            }

            return Ok(lsCourses);
        }


        /// <summary>
        /// GET: api/Courses/5
        /// Get sökt course
        /// </summary>
        /// <param name="id">id för sökt course</param>
        /// <returns>Ok = 200 och sökt course eller NotFound = 404</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            CourseDto dto = null;
            //var course = await _context.Course.FindAsync(id);
            var course = await m_Uow.CourseRepository.GetCourseAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            else
            {
                dto = m_Mapper.Map<CourseDto>(course);
            }

            return Ok(dto);
        }

        /// <summary>
        /// PUT: api/Courses/5
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// Put, update/replace, course
        /// </summary>
        /// <param name="id">id för den course som skall uppdateras</param>
        /// <param name="courseDto">Information om course som skall uppdateras</param>
        /// <returns>Om det gick bra returneras Ok = 200. 
        /// Om id och coursedto ej har samma id returneras BadRequest = 400. 
        /// Om ModelState ej är valid returneras BadRequest = 400.
        /// Om course som skall uppdateras inte finns returneras NotFound = 404.
        /// Om det inte gick uppdatera course returneras StatusCode = 500
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseAsync(int id, CourseDto courseDto)
        {
            if (id != courseDto.CourseId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //_context.Entry(course).State = EntityState.Modified;

            try
            {
                Course course = m_Mapper.Map<Course>(courseDto);
                m_Uow.CourseRepository.PutCourse(id, course);
                //await _context.SaveChangesAsync();
                bool bSaveOk = await m_Uow.CourseRepository.SaveAsync();

                if (!bSaveOk)
                    return StatusCode(500);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool bCourseExists = await CourseExistsAsync(id);
                if (!bCourseExists)
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
        /// POST: api/Courses
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// Post, create, ny course
        /// </summary>
        /// <param name="courseDto">Information om course som skall skapas</param>
        /// <returns>Om det gick bra sker det en redirect till GetCourse action.
        /// Om ModelState ej är valid returneras BadRequest = 400.
        /// Om det inte gick att skapa course returneras StatusCode = 500
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto courseDto)
        {
            //_context.Course.Add(course);
            //await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Course course = m_Mapper.Map<Course>(courseDto);

            await m_Uow.CourseRepository.AddAsync(course);
            bool bSaveOk = await m_Uow.CourseRepository.SaveAsync();

            if (!bSaveOk)
                return StatusCode(500);

            return CreatedAtAction("GetCourse", new { id = courseDto.CourseId }, courseDto);
        }


        /// <summary>
        /// DELETE: api/Courses/5
        /// Delete, radera, course
        /// </summary>
        /// <param name="id">id för den course som skall raderas</param>
        /// <returns>Om det gick bra returneras Ok = 200. 
        /// Om course som skall raderas inte finns returneras NotFound = 404.
        /// Om det inte gick att skapa course returneras StatusCode = 500.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            //var course = await _context.Course.FindAsync(id);
            var course = await m_Uow.CourseRepository.GetCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            //_context.Course.Remove(course);
            //await _context.SaveChangesAsync();

            // Radera course
            await m_Uow.CourseRepository.DeleteAsync(id);
            bool bSaveOk = await m_Uow.CourseRepository.SaveAsync();
            if (!bSaveOk)
                return StatusCode(500);

            return Ok();
        }


        /// <summary>
        /// Metoden testar om en course finns
        /// </summary>
        /// <param name="id">id för sökt course</param>
        /// <returns>true om sökt course finns. Annars returneras false</returns>
        private async Task<bool> CourseExistsAsync(int id)
        {
            bool bCourseExists = true;
            
            var course = await m_Uow.CourseRepository.GetCourseAsync(id);
            if (course is null)
                bCourseExists = false;

            //return _context.Course.Any(e => e.CourseId == id);
            return bCourseExists;
        }


        /// <summary>
        /// Patch, update/modify, course
        /// </summary>
        /// <param name="courseId">id för course som skall uppdateras</param>
        /// <param name="patchDocument"></param>
        /// <returns>Om det gick bra returneras Ok = 200 och information om course.
        /// Om course inte finns returneras NotFound = 404.
        /// Om ModelState ej är valid returneras BadRequest = 400.
        /// Om det inte gick att uppdatera course returneras StatusCode = 500
        /// </returns>
        /// <remarks>
        /// Sample request. Uppdaterar Titel med value \
        /// PATCH /api/Courses/16 \
        /// [ \
        ///    { \
        ///         "op": "replace", \
        ///         "path": "/Title", \
        ///         "value": "Dude new title" \
        ///     } \
        /// ]
        /// </remarks>
        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            Course course = await m_Uow.CourseRepository.GetCourseAsync(courseId);
            if (course is null)
                return NotFound();

            CourseDto dto = m_Mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(dto, ModelState);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState);

            m_Mapper.Map(dto, course);

            bool bSaveOk = await m_Uow.CourseRepository.SaveAsync();
            if (bSaveOk)
            {
                dto = m_Mapper.Map<CourseDto>(course);
                return Ok(dto);
            }

            return StatusCode(500);
        }
    }
}