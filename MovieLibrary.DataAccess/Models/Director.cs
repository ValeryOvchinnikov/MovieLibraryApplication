using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.DataAccess.Models
{
    public class Director
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual IList<Movie> Movies { get; private set; } = new List<Movie>();
    }
}
