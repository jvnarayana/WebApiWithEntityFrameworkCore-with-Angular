using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WebApplication1.Entities;

namespace WebApplication1.Repositories;

public interface IGenericRepository<T> where T: class
{
    Task<T> GetByIdAsync(int id);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    IQueryable<T?> GetAll();
    Task<IEnumerable<T?>> GetAllAsync();

    Task AddAsync(T? entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
public class GenericRepository<T> : IGenericRepository<T> where T: class
{
     readonly StudentsDBContext _dbContext;
     readonly DbSet<T?> _dbSet;

    public GenericRepository(StudentsDBContext dbContext)
    {
        this._dbContext = dbContext;
        this._dbSet = dbContext.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T?> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }
    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public IQueryable<T?> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task AddAsync(T? entity)
    {
        await _dbSet.AddAsync(entity); 
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    
}