using MvpVendingMachineApp.Api.Controllers.v1.Products.Requests;
using FluentValidation;

namespace MvpVendingMachineApp.Api.Controllers.v1.Products.Validators
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Cost)
              .NotNull().NotEmpty().GreaterThan(0).Must(e => e % 5 == 0).WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.AmountAvailable).GreaterThan(0).WithMessage("[PropertyName] is not valid");
        }
    }
}
