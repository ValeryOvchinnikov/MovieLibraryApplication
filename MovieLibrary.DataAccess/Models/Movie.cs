using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.DataAccess.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Name { get; set; }

        public double Rating { get; set; }

        [ForeignKey("Director")]
        public int DirectorId { get; set; }

        public virtual Director? Director { get; set; }

        public virtual IList<Rating> Ratings { get; private set; } = new List<Rating>();

        public virtual IList<Comment> Comments { get; private set; } = new List<Comment>();

        public int Year { get; set; }
    }
}
