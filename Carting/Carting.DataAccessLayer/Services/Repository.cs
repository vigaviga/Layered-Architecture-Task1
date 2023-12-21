using Carting.Carting.Application.Interfaces;
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
        public async Task<string> Add(T entity)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.Insert(entity).AsString;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.FindAll();
            }
        }

        public async Task<T> GetById(string Id)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                return collection.FindById(new BsonValue(Id));
            }
        }

        public async Task Remove(string Id)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                collection.Delete(new BsonValue(Id));
            }
        }

        public async Task Update(T entity)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var collection = db.GetCollection<T>();
                collection.Update(entity);
            }
        }
    }
}
