using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using notepad.app.Abstract;
using notepad.business.Abstract;
using notepad.business.Dto_s.Notes;
using notepad.entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using notepad.business.Validator;
using notepad.entity.Entities.Identity;

namespace notepad.business.Concrete;

public class NoteService : INoteService
{
    readonly private INoteReadRepository _noteReadRepository;
    readonly private INoteWriteRepository _noteWriteRepository;
    readonly private IMailService _mailService;
    readonly private IHttpContextAccessor _httpContextAccessor;
    readonly private IFileService _fileService;
    readonly private UserManager<AppUser> _userManager;

    public NoteService(INoteReadRepository noteReadRepository, INoteWriteRepository noteWriteRepository,
        IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IFileService fileService, IMailService mailService)
    {
        _noteReadRepository = noteReadRepository;
        _noteWriteRepository = noteWriteRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _fileService = fileService;
        _mailService = mailService;
    }

    public async Task<ICollection<Note>> GetAllAsync()
    {
        var notes = await _noteReadRepository.GetAll()
            .Include(a => a.AppUser)
            .ToListAsync();
        return notes;
    }

    public async Task<Note> GetById(string id)
    {
        var note = await _noteReadRepository.GetById(id)
                   ?? throw new NotFoundException("Values not found ");
        return note;
    }

    public async Task<bool> EmailNotification()
    {
      
        var notes = await _noteReadRepository.GetWhere(a => a.NotifiedDateTime <= DateTime.UtcNow && a.NotifiedDateTime != null).ToListAsync();

        foreach (var note in notes)
        {
          
            var userEmail = new[] { note.AppUser.Email };

          
            var singleNote = new List<Note> { note };

          
            await _mailService.NotificationEmailAsync(userEmail, singleNote);
        }

     
        return true;
    }


    public async Task<bool> Create(CreateNoteDto model)
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name
                       ?? throw new NotFoundException($"User name  is not valid.");
        AppUser user = await _userManager.FindByNameAsync(userName)
                       ?? throw new NotFoundException($"User  is not valid.");
        Note note = new Note();
        if (model.FormFile != null)
        {
            bool isImage = _fileService.IsImage(model.FormFile);
            bool isVideo = _fileService.IsVideo(model.FormFile);
            if (!isImage || !isVideo)
            {
                throw new InvalidFileTypeException("The uploaded file is neither an image nor a video.");
            }


            if (isImage)
            {
                var newImage = await _fileService.UploadAsync(model.FormFile);
                note.PhotoName = newImage;
            }
            else if (isVideo)
            {
                var newVideo = await _fileService.UploadVideoAsync(model.FormFile);
                note.VideoName = newVideo;
            }
        }

        note.Title = model.Title;
        note.Description = model.Description;

        note.NotifiedDateTime = model.NotifiedDateTime;
        note.isNotified = model.isNotified;
        note.AppUserId = user.Id;

        await _noteWriteRepository.AddAsync(note);
        await _noteWriteRepository.SaveAsync();

        return true;
    }

    public async Task<bool> Update(UpdateNoteDto model)
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name
                       ?? throw new NotFoundException($"User name  is not valid.");
        AppUser user = await _userManager.FindByNameAsync(userName)
                       ?? throw new NotFoundException($"User  is not valid.");
        var note = await _noteReadRepository.GetById(model.id) ??
                   throw new NotFoundException($"User name  is not valid.");
        if (model.FormFile != null)
        {
            bool isImage = _fileService.IsImage(model.FormFile);
            bool isVideo = _fileService.IsVideo(model.FormFile);
            if (!isImage || !isVideo)
            {
                throw new InvalidFileTypeException("The uploaded file is neither an image nor a video.");
            }


            if (isImage)
            {
                var newImage = await _fileService.UploadAsync(model.FormFile);
                note.PhotoName = newImage;
            }
            else if (isVideo)
            {
                var newVideo = await _fileService.UploadVideoAsync(model.FormFile);
                note.VideoName = newVideo;
            }
        }

        note.Title = model.Title;
        note.Description = model.Description;

        note.NotifiedDateTime = model.NotifiedDateTime;
        note.isNotified = model.isNotified;
        note.AppUserId = user.Id;

        await _noteWriteRepository.Update(note);
        await _noteWriteRepository.SaveAsync();

        return true;
    }

    public async Task<bool> DeleteDays(string id)
    {
        var note = await _noteReadRepository.GetById(id)
                   ?? throw new NotFoundException("Value not found.");

        if (note.isDeleted)
        {
            note.isDeleted = true;
            note.DeletedDate = DateTime.Now.AddDays(30);
        }
        else
        {
            note.isDeleted = false;
            note.DeletedDate = DateTime.MinValue;
        }

        await _noteWriteRepository.Update(note);
        await _noteWriteRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteRange(bool isDeleted)
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name
                       ?? throw new NotFoundException($"User name  is not valid.");
        AppUser user = await _userManager.FindByNameAsync(userName)
                       ?? throw new NotFoundException($"User  is not valid.");

        var notes = await _noteReadRepository
            .GetWhere(a => a.isNotified == isDeleted && a.AppUserId == user.Id)
            .ToListAsync() ?? throw new NotFoundException("Values is not found");

        _noteWriteRepository.DeleteRange(notes);
        await _noteWriteRepository.SaveAsync();

        return true;
    }

    public async Task<bool> UpdateRange(bool isDeleted)
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name
                       ?? throw new NotFoundException($"User name  is not valid.");
        AppUser user = await _userManager.FindByNameAsync(userName)
                       ?? throw new NotFoundException($"User  is not valid.");
        var notes = await _noteReadRepository.GetWhere(a => a.isNotified == isDeleted && a.AppUserId == user.Id)
            .ToListAsync();

        foreach (var a in notes)
        {
            a.isNotified = false;
            a.DeletedDate = DateTime.MinValue;
        }

        await _noteWriteRepository.UpdateRange(notes);
        await _noteWriteRepository.SaveAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        var note = await _noteReadRepository.GetById(id)
                   ?? throw new NotFoundException("Value not found.");


        _noteWriteRepository.Remove(note);
        await _noteWriteRepository.SaveAsync();
        return true;
    }
}