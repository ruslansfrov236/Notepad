using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity.Entities.Common;

namespace notepad.app.Concrete;

public class ReadRepository<T> :IReadRepository<T> where T:BaseEntity
{
    readonly private AppDbContext _context;

    public ReadRepository(AppDbContext context)
    {
        _context = context;
    }

    private DbSet<T> Table => _context.Set<T>();
    public IQueryable<T> GetAll(bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        return query;
    }

    public async Task<T> GetById(string id, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(a=>a.Id==Guid.Parse(id));
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        return query.Where(method);
    }

    public async Task<T> GetSingle(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(method);
    }
}