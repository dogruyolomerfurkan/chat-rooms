using Chatter.Domain.Entities.EFCore.Application.Base;
using Chatter.Persistence.RepositoryManagement.Base;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Base;

public class EfCoreBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>, new()
    where TKey : IComparable
{
    private readonly DbContext _context;

    public EfCoreBaseRepository(DbContext context)
    {
        _context = context;
        _context.ChangeTracker.LazyLoadingEnabled = true;
    }

    public async Task CreateAsync(List<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }


    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        SaveChanges();
    }

    public async Task<TEntity> FindAsync(TKey entityId)
    {
        return await _context.Set<TEntity>().FindAsync(entityId);
    }

    public IQueryable<TEntity> Query(bool asNoTracking = false)
    {
        return asNoTracking
            ? _context.Set<TEntity>().AsNoTracking()
            : _context.Set<TEntity>();
    }


    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}