using AutoMapper;
using MinimalApisCourseGavilanNet8.DTOs;
using MinimalApisCourseGavilanNet8.Entities;

namespace MinimalApisCourseGavilanNet8.Utilities
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenreDTO, Genre>();
        }
    }
}
