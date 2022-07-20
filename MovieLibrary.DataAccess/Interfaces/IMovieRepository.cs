using System;
using LinqKit;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.DataAccess.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetWhere(ExpressionStarter<Movie> condition);
        Task CreateRangeAsync(List<Movie> movies);

    }
}
