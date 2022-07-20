using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
