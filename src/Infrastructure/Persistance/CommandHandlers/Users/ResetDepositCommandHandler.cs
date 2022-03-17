using MediatR;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Domain.IRepositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class ResetDepositCommandHandler : IRequestHandler<ResetDepositCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ResetDepositCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(ResetDepositCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, request.UserId);
            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "user not found");
            user.Deposit = 0;
            _userRepository.Update(user);

            return Unit.Value;
        }
    }
}
