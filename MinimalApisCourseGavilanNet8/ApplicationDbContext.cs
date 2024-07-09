using Microsoft.EntityFrameworkCore;

namespace MinimalApisCourseGavilanNet8
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
