using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity.Entities;

namespace notepad.app.Concrete;

public class NoteReadRepository:ReadRepository<Note> , INoteReadRepository
{
    public NoteReadRepository(AppDbContext context) : base(context)
    {
    }
}