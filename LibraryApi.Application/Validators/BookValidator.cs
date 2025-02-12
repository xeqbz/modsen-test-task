using FluentValidation;
using LibraryApi.Application.Requests;

namespace LibraryApi.Application.Validators
{
    public class BookValidator : AbstractValidator<CreateBookRequest>
    {
        public BookValidator()
        {
            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required")
                .Length(10, 20).WithMessage("ISBN must be 10 <= x <= 20 characters");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters");
            RuleFor(x => x.Genre)
                .MaximumLength(255);
            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}
