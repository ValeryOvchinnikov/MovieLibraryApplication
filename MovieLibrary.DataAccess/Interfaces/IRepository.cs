namespace MovieLibrary.DataAccess.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<IList<T>?> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<int> CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
