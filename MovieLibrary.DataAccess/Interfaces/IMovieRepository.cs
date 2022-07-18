using System;
using LinqKit;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetWhere(ExpressionStarter<Movie> condition);
    }
}
