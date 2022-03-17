using FluentValidation;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;

namespace MvpVendingMachineApp.Api.Controllers.v1.Products.Validators
{
    public class UserBuyProductRequestValidator : AbstractValidator<UserBuyProductRequest>
    {
        public UserBuyProductRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);

        }
    }
}
