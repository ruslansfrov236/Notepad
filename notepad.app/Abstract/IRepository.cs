using Microsoft.EntityFrameworkCore;
using notepad.entity.Entities.Common;

namespace notepad.app.Abstract;

public interface IRepository<T> where T:BaseEntity
{ 
    DbSet<T> Table { get; }
}