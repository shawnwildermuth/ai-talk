using FluentValidation;
using SnackBarPOS.Api.Models.Requests;

namespace SnackBarPOS.Api.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naam is verplicht")
            .MaximumLength(200).WithMessage("Naam mag maximaal 200 tekens zijn");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Beschrijving mag maximaal 500 tekens zijn")
            .When(x => x.Description is not null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Prijs moet groter zijn dan 0")
            .LessThan(10000).WithMessage("Prijs mag niet hoger zijn dan €10.000");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Categorie is verplicht");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Afbeelding URL mag maximaal 500 tekens zijn")
            .When(x => x.ImageUrl is not null);
    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naam is verplicht")
            .MaximumLength(200).WithMessage("Naam mag maximaal 200 tekens zijn");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Beschrijving mag maximaal 500 tekens zijn")
            .When(x => x.Description is not null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Prijs moet groter zijn dan 0")
            .LessThan(10000).WithMessage("Prijs mag niet hoger zijn dan €10.000");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Categorie is verplicht");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Afbeelding URL mag maximaal 500 tekens zijn")
            .When(x => x.ImageUrl is not null);
    }
}
