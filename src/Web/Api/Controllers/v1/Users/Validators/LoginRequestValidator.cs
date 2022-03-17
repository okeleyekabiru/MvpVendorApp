using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using FluentValidation;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        }
    }
}
