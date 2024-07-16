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
    }
}
