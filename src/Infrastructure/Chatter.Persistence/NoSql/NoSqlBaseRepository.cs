using Chatter.Common.Settings;
using Chatter.Domain.Entities.NoSql;
using Chatter.Domain.Entities.NoSql.Base;
using Chatter.Persistence.Base;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chatter.Persistence.NoSql;

public class NoSqlBaseRepository<TEntity> : IBaseRepository<TEntity, string>
    where TEntity : BaseEntity, new()
{
    protected readonly IMongoCollection<TEntity> _collection;
    protected readonly MongoDbSettings _mongoDbSettings;

    public NoSqlBaseRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings.Value;
        var client = new MongoClient(_mongoDbSettings.ConnectionString);
        var database = client.GetDatabase(_mongoDbSettings.DatabaseName);
        _collection = database.GetCollection<TEntity>(typeof(TEntity).Name.ToLowerInvariant());
    }
    public async Task CreateAsync(List<TEntity> entites)
    {
        await _collection.InsertManyAsync(entites);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        _collection.FindOneAndReplace(x => x.Id == entity.Id, entity);
    }

    public void Delete(TEntity entity)
    {
        _collection.DeleteOne(x => x.Id == entity.Id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(x => true).ToListAsync(); 
    }

    public async Task<TEntity> FindAsync(string entityId)
    {
        return await _collection.Find(x => x.Id == entityId).FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> Query()
    {
        return _collection.AsQueryable();
    }
}