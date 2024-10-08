﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalApisCourseGavilanNet8.DTOs;
using MinimalApisCourseGavilanNet8.Entities;
using MinimalApisCourseGavilanNet8.Repositories;
using MinimalApisCourseGavilanNet8.Services;

namespace MinimalApisCourseGavilanNet8.Endpoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            group.MapPost("/",Create).DisableAntiforgery();
            return group;
        }

       static async Task<Created<ActorDTO>> Create([FromForm] CreateActorDTO createActorDTO,
           IActorsRepository repository, IOutputCacheStore outputCacheStore, IMapper mapper,
           IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(createActorDTO);

            if (createActorDTO.Picture is not null) {
                var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }

            var id = await repository.Create(actor);

            await outputCacheStore.EvictByTagAsync("actors-get", default);

            var actorDTO = mapper.Map<ActorDTO>(actor);

            return TypedResults.Created($"/actors/{id}",actorDTO);
        }
    }
}
