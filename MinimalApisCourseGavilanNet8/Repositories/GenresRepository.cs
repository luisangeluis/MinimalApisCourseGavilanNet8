﻿using Microsoft.EntityFrameworkCore;
using MinimalApisCourseGavilanNet8.Entities;

namespace MinimalApisCourseGavilanNet8.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly ApplicationDbContext context;

        public GenresRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }

        public async Task Delete(int id)
        {
            await context.Genres.Where(g => g.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Genres.AnyAsync(gen => gen.Id == id);
        }

        public async Task<List<Genre>> GetAll()
        {
            //return await context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return await context.Genres.OrderByDescending(g=>g.Name).ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task Update(Genre genre)
        {
            context.Update(genre);
            await context.SaveChangesAsync();
        }

        

        
    }
}
