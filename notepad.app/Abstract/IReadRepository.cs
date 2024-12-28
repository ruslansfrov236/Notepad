using System.Linq.Expressions;
using notepad.entity.Entities.Common;

namespace notepad.app.Abstract;

public interface IReadRepository<T> where T :BaseEntity
{
    IQueryable<T> GetAll(bool tracking = true);
    Task<T> GetById(string id, bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetSingle(Expression<Func<T, bool>> method, bool tracking = true);

}