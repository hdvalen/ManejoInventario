namespace ManejoInventario.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Operaciones básicas CRUD
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(object id);
    }
}