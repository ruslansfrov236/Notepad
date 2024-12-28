using FluentValidation;
using notepad.business.Dto_s.Notes;

namespace notepad.business.Validator.Filter;

public class CreateNoteValidator:AbstractValidator<CreateNoteDto>
{
    public CreateNoteValidator()
    {
        RuleFor(a=>a.Title)
            .NotEmpty()
            .WithMessage("Please do not enter blank information.")
            .MaximumLength(250)
            .WithMessage("Please enter information between maximum 250 characters.")
            .MinimumLength(4)
            .WithMessage("Please enter information between minimum 4 characters.");
        RuleFor(a=>a.Description)
            .NotEmpty()
            .WithMessage("Please do not enter blank information.")
            .MaximumLength(250)
            .WithMessage("Please enter information between maximum 250 characters.")
            .MinimumLength(4)
            .WithMessage("Please enter information between minimum 4 characters.");
       
    }
    
}