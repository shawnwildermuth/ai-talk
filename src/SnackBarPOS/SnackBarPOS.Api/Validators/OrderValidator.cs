using FluentValidation;
using SnackBarPOS.Api.Models.Requests;

namespace SnackBarPOS.Api.Validators;

public class AddOrderItemValidator : AbstractValidator<AddOrderItemRequest>
{
    public AddOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product is verplicht");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Aantal moet groter zijn dan 0")
            .LessThanOrEqualTo(100).WithMessage("Aantal mag maximaal 100 zijn");
    }
}

public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemRequest>
{
    public UpdateOrderItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Aantal moet 0 of hoger zijn")
            .LessThanOrEqualTo(100).WithMessage("Aantal mag maximaal 100 zijn");
    }
}

public class PayOrderValidator : AbstractValidator<PayOrderRequest>
{
    private static readonly string[] ValidPaymentMethods = ["Cash", "Pin", "Contactless"];

    public PayOrderValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Betaalwijze is verplicht")
            .Must(m => ValidPaymentMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Ongeldige betaalwijze. Kies uit: Cash, Pin, Contactless");

        RuleFor(x => x.AmountTendered)
            .GreaterThan(0).WithMessage("Betaald bedrag moet groter zijn dan 0")
            .When(x => x.AmountTendered.HasValue);
    }
}
