using FluentValidation;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Validators
{
    public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
    {

        public DeleteUserRequestValidator()
        {
            RuleFor(e => e.UserId).GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
