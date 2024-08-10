namespace MinimalApisCourseGavilanNet8.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        public DateTime DateOfBirt { get; set; }
        public string? Picture { get; set; }
    }
}
