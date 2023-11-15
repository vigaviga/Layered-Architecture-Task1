using Carting.DataAccessLayer.Interfaces;
using LiteDB;

namespace Carting.DataAccessLayer.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _databasePath;
        public Repository(string databasePath)
        {
            _databasePath = databasePath;
        }
        public int Add(T entity)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.Insert(entity).AsInt32;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.FindAll();
            }
        }

        public T GetById(int Id)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.FindById(new BsonValue(Id));
            }
        }

        public void Remove(int Id)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                collection.Delete(new BsonValue(Id));
            }
        }

        public void Update(T entity)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                collection.Update(entity);
            }
        }
    }
}
