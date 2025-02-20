using BookManagementApi.Domain.DTOs;
using FluentValidation;

namespace BookManagementApi.Validators;

public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Book title is required")
            .MaximumLength(100).WithMessage("Book title must not exceed 100 characters");

        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(100).WithMessage("Author name must not exceed 100 characters");

        RuleFor(x => x.PublicationYear)
            .NotEmpty().WithMessage("Publication year is required")
            .GreaterThan(0).WithMessage("Publication year must be greater than 0")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Publication year must be less than or equal to the current year");

        RuleFor(x => x.IsDeleted)
            .NotNull().WithMessage("IsDeleted is required. IsDeleted must be a boolean value (true or false)");
    }
}
