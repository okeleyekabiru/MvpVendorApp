using MediatR;

namespace MvpVendingMachineApp.Application.Users.Command
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }


    }
}
