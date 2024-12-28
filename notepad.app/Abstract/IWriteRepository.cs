using notepad.entity.Entities.Common;

namespace notepad.app.Abstract;

public interface IWriteRepository<T> where T:BaseEntity
{
    Task<bool> AddAsync(T model);
    Task<bool> Update(T model);
    Task<bool> UpdateRange(List<T> model);
    Task<bool> DeleteRange(List<T> model );
    Task<bool> DeleteAsync(string id);
    bool Remove(T model);
    Task<int> SaveAsync();
}