using MvpVendingMachineApp.Domain.Entities.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Common.Utilities;
using MvpVendingMachineApp.Domain.IRepositories;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordhash = SecurityHelper.GetSha256Hash(request.Password);
            var user = new User
            {
                UserName = request.UserName,
                Role = request.Role,
                PasswordHash = passwordhash
            };

            await _userRepository.AddAsync(user, cancellationToken, true);

            return true;
        }
    }
}
