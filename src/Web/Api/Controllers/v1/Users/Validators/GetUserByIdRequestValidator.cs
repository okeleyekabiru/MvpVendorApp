using FluentValidation;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Validators
{
    public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
    {

        public GetUserByIdRequestValidator()
        {
            RuleFor(e => e.UserId).GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
