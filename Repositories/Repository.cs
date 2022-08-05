using System.Linq.Expressions;
using System.Linq;
using MongoDB.Driver;
using MongoDbDemo.Attributes;
using MongoDbDemo.Entities;

namespace MongoDbDemo.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly IMongoCollection<TEntity> collection;
        private readonly IConfiguration configuration;

        public Repository(IConfiguration configuration)
        {
            var database = new MongoClient(configuration["MongoDbSettings:ConnectionString"]).GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
            this.configuration = configuration;
        }

        private protected string GetCollectionName(Type type)
        {
            return ((BsonCollectionAttribute)type.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true).FirstOrDefault())?.CollectionName;
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var entity = await collection.Find(filter).FirstOrDefaultAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await Task.Run(() => collection.AsQueryable().AsEnumerable().ToList());
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await Task.Run(() => collection.AsQueryable().Where(predicate).Select(s => s).AsEnumerable());
            return entities;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await collection.InsertOneAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await collection.DeleteOneAsync(filter);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await collection.FindOneAndReplaceAsync(filter, entity);
        }
    }
}