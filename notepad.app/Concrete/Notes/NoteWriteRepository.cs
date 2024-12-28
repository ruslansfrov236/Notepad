using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity.Entities;

namespace notepad.app.Concrete;

public class NoteWriteRepository:WriteRepository<Note> , INoteWriteRepository
{
    public NoteWriteRepository(AppDbContext context) : base(context)
    {
    }
}