namespace MovieLibrary.BusinessLogic.Models
{
    public class MovieDTO
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
