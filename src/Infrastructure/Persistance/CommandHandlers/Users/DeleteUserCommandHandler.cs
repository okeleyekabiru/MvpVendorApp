using MediatR;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);

            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "User does not exist");

            _userRepository.DeleteAsync(user, cancellationToken, true);
            return Task.FromResult(Unit.Value);

        }
    }
}
