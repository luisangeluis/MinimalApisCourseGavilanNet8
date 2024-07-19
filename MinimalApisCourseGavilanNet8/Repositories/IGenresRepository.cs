using MinimalApisCourseGavilanNet8.Entities;

namespace MinimalApisCourseGavilanNet8.Repositories
{
    public interface IGenresRepository
    {
        Task<int> Create(Genre genre);
        Task<Genre?> GetById(int id);
        Task<List<Genre>> GetAll();
    }
}
