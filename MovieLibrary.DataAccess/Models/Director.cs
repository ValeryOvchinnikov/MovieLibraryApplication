using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.DataAccess.Models
{
    public class Director
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual IList<Movie> Movies { get; private set; } = new List<Movie>();
    }
}
