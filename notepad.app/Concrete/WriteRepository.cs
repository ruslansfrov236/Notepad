using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity.Entities.Common;

namespace notepad.app.Concrete;

public class WriteRepository<T> :IWriteRepository<T> where T:BaseEntity
{
    readonly private AppDbContext _context;

    public WriteRepository(AppDbContext context)
    {
        _context = context;
    }

    private DbSet<T> Table => _context.Set<T>();
    public async Task<bool> AddAsync(T model)
    {
        EntityEntry<T> entityEntry = await Table.AddAsync(model);
        return entityEntry.State == EntityState.Added;
    }

    public async Task<bool> Update(T model)
    {
        EntityEntry<T> entityEntry =  Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }

    public async Task<bool> UpdateRange(List<T> model)
    {
       Table.UpdateRange(model);

       return true;
    }

    public async Task<bool> DeleteRange(List<T> model)
    {
        Table.RemoveRange(model);
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    { 
        T? entity = await Table.FindAsync(Guid.Parse(id));
        if (entity == null) return false;

        return Remove(entity);
    }

    public bool Remove(T model)
    {
        EntityEntry<T> entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }

    public Task<int> SaveAsync()
        => _context.SaveChangesAsync();
}