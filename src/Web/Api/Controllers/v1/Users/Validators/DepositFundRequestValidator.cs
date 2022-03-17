using FluentValidation;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using System.Collections.Generic;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Validators
{
    public class DepositFundRequestValidator : AbstractValidator<DepositFundRequest>
    {
        private List<decimal> _allowedDepositAmount = new List<decimal> { 5, 10, 20, 50, 100 };
        public DepositFundRequestValidator()
        {

            RuleFor(e => e.Amount).GreaterThan(0).Must(e => _allowedDepositAmount.Contains(e));
        }
    }
}
