using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Persistance.Jwt;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Application.Users.Command.Response;
using MvpVendingMachineApp.Domain.IRepositories;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUserRepository userRepository,
                                   IJwtService jwtService)
        {
            _userRepository = userRepository ?? throw new System.ArgumentNullException(nameof(_userRepository));
            _jwtService = jwtService ?? throw new System.ArgumentNullException(nameof(jwtService));
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserAndPass(request.Username, request.Password, cancellationToken);

            if (user == null)
                throw new VendorAppException("Invalid username or password");

            var jwt = await _jwtService.GenerateAsync(user);

            return new LoginResponse
            {
                AccessToken = jwt.access_token,
                RefreshToken = jwt.refresh_token
            };
        }
    }
}
