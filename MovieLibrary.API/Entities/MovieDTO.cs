namespace MovieLibrary.API.Entities
{
#pragma warning disable S101 // Types should be named in PascalCase
    public class MovieDTO
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Year { get; set; }

        public int Rating { get; set; }

        public int DirectorId { get; set; }

        public string? DirectorFirstName { get; set; }

        public string? DirectorLastName { get; set; }
    }
}
