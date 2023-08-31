using Chatter.Domain.Entities.EFCore.Application.Base;
using Chatter.Persistence.RepositoryManagement.Base;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Base;

public class EfCoreBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>, new()
    where TKey : IComparable
{
    private readonly DbContext _context;
    protected DbSet<TEntity> DbSet { get; set; }

    public EfCoreBaseRepository(DbContext context)
    {
        _context = context;

        _context.ChangeTracker.LazyLoadingEnabled = true;
        DbSet = _context.Set<TEntity>();
    }

    public async Task CreateAsync(List<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        _context.SaveChanges();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        _context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        _context.SaveChanges();
    }

    public async Task<TEntity> FindAsync(TKey entityId)
    {
        return await _context.Set<TEntity>().FindAsync(entityId);
    }

    public IQueryable<TEntity> Query()
    {
        return _context.Set<TEntity>().AsQueryable();
    }


    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
}
