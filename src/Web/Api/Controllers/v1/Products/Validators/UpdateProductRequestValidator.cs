using FluentValidation;
using MvpVendingMachineApp.Api.Controllers.v1.Products.Requests;

namespace MvpVendingMachineApp.Api.Controllers.v1.Products.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} is not valid");
        }

    }
}


