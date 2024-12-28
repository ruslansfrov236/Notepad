using notepad.business.Dto_s.Notes;
using notepad.entity.Entities;

namespace notepad.business.Abstract;

public interface INoteService
{
    Task<ICollection<Note>> GetAllAsync();
    Task<Note> GetById(string id);
    Task<bool> Create(CreateNoteDto model);
    Task<bool> Update(UpdateNoteDto model);
    Task<bool> DeleteDays(string id);
    Task<bool> DeleteRange(bool isDeleted);
    Task<bool> UpdateRange(bool isDeleted);

    Task<bool> Delete(string id );
}