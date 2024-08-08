﻿using MinimalApisCourseGavilanNet8.Entities;


namespace MinimalApisCourseGavilanNet8.Repositories
{
    public interface IActorsRepository
    {
        Task<int> Create(Actor actor);
        Task Delete(int id);
        Task<bool> Exist(int id);
        Task<List<Actor>> GetAll();
        Task<Actor?> GetById(int id);
        Task Update(Actor actor);
    }
}