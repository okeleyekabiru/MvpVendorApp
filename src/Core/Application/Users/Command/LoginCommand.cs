using MediatR;
using MvpVendingMachineApp.Application.Users.Command.Response;

namespace MvpVendingMachineApp.Application.Users.Command
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Refresh_token { get; set; }
    }
}
