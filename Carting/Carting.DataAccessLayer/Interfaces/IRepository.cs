namespace Carting.DataAccessLayer.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int Id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        void Update(T entity);
        void Remove(int Id);
    }
}
