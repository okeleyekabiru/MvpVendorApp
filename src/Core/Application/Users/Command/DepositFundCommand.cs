using MediatR;
using MvpVendingMachineApp.Application.Users.Response;

namespace MvpVendingMachineApp.Application.Users.Command
{
    public class DepositFundCommand : IRequest<DepositFundResponse>
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
