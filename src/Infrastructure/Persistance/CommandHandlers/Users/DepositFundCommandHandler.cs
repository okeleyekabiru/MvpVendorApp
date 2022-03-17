using MediatR;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Application.Users.Response;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class DepositFundCommandHandler : IRequestHandler<DepositFundCommand, DepositFundResponse>
    {
        private readonly IUserRepository _userRepository;

        public DepositFundCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<DepositFundResponse> Handle(DepositFundCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, request.UserId);

            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "user not found");

            user.Deposit += request.Amount;
            _userRepository.Update(user, true);

            return new DepositFundResponse { Balance = user.Deposit, Deposit = request.Amount };

        }
    }
}
