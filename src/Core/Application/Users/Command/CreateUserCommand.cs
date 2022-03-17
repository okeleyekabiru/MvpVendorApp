using MvpVendingMachineApp.Domain.Entities.Users;
using MediatR;

namespace MvpVendingMachineApp.Application.Users.Command
{
    public class CreateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }


    }
}
