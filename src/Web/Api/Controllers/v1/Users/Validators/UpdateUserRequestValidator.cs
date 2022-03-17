using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using FluentValidation;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.NewPassword)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        }
    }
}
