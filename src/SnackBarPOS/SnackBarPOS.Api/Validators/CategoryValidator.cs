using FluentValidation;
using SnackBarPOS.Api.Models.Requests;

namespace SnackBarPOS.Api.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naam is verplicht")
            .MaximumLength(100).WithMessage("Naam mag maximaal 100 tekens zijn");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Beschrijving mag maximaal 500 tekens zijn")
            .When(x => x.Description is not null);

        RuleFor(x => x.IconEmoji)
            .MaximumLength(10).WithMessage("Emoji mag maximaal 10 tekens zijn")
            .When(x => x.IconEmoji is not null);

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sorteervolgorde moet 0 of hoger zijn");
    }
}

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naam is verplicht")
            .MaximumLength(100).WithMessage("Naam mag maximaal 100 tekens zijn");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Beschrijving mag maximaal 500 tekens zijn")
            .When(x => x.Description is not null);

        RuleFor(x => x.IconEmoji)
            .MaximumLength(10).WithMessage("Emoji mag maximaal 10 tekens zijn")
            .When(x => x.IconEmoji is not null);

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sorteervolgorde moet 0 of hoger zijn");
    }
}
