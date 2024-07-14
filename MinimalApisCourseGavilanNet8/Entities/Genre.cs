using System.ComponentModel.DataAnnotations;

namespace MinimalApisCourseGavilanNet8.Entities
{
    public class Genre
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
    }
}
