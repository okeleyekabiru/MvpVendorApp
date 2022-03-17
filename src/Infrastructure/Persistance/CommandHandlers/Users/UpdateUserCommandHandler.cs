using MediatR;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.Utilities;
using MvpVendingMachineApp.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, request.Id);
            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "User does not exist");
            var oldPassswordHash = SecurityHelper.GetSha256Hash(request.OldPassword);
            if (user.PasswordHash == oldPassswordHash)
                user.PasswordHash = SecurityHelper.GetSha256Hash(request.NewPassword);

            _userRepository.Update(user, true);

            return Unit.Value;

        }
    }
}