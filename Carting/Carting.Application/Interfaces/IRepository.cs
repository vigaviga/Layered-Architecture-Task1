namespace Carting.Carting.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(string Id);
        Task<IEnumerable<T>> GetAll();
        Task<string> Add(T entity);
        Task Update(T entity);
        Task Remove(string Id);
    }
}
