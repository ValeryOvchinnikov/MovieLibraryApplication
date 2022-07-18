using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.BusinessLogic.Models
{
#pragma warning disable S101 // Types should be named in PascalCase
    public class DirectorDTO
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Collection<MovieDTO> Movies { get; private set; } = new Collection<MovieDTO>();
    }
}
