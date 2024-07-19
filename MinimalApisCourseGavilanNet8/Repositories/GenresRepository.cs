using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Genre>> GetAll()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
