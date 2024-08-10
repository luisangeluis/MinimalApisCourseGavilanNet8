
namespace MinimalApisCourseGavilanNet8.Services
{
    public class LocalFileStorage(IWebHostEnvironment env,
        IHttpContextAccessor httpContextAccessor) : IFileStorage
    {
        public Task Delete(string? route, string container)
        {
            throw new NotImplementedException();
        }

        public Task<string> Store(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath,container);
        }
    }
}
