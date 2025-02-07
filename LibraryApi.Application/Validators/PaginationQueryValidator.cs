using FluentValidation;
using LibraryApi.Domain.Common;

namespace LibraryApi.Application.Validators
{
    public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
    {
        public PaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("PageSize must be between 1 and 50");
        }
    }
}
