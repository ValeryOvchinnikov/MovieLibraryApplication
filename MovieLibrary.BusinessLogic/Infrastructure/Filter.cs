using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.BusinessLogic.Infrastructure
{
    public class Filter
    {
        public string FilterMovieName { get; set; } = "";

        public string FilterMovieYear { get; set; } = "";

        public string FilterDirectorFirstName { get; set; } = "";

        public string FilterDirectorLastName { get; set; } = "";

        public string FilterMovieRating { get; set; } = "";
    }
}
