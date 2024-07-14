using Microsoft.EntityFrameworkCore;
using MinimalApisCourseGavilanNet8.Entities;

namespace MinimalApisCourseGavilanNet8
{
    public class ApplicationDbContext : DbContext
    {   
        //Agregar una migracion
        //Add-Migration nombredelamigracion

        //Apply migrations in our database
        //Update-Database
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
