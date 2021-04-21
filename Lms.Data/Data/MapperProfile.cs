using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Models.Entities;

namespace Lms.Data.Data
{
    /// <summary>
    /// Automapper konfiguration
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public MapperProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ForMember(dest => dest.ModuleId, from => from.MapFrom(a => a.Id));
            CreateMap<ModuleDto, Module>().ForMember(dest => dest.Id, from => from.MapFrom(a => a.ModuleId));
        }
    }
}