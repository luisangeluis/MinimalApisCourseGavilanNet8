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

            CreateMap<Actor,ActorDTO>();
            CreateMap<CreateActorDTO, Actor>()
                .ForMember(p=>p.Picture,options=>options.Ignore());
        }
    }
}
