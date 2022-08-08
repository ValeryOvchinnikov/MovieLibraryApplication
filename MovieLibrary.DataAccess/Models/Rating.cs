using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.DataAccess.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserNickname { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int Value { get; set; }
    }
}
