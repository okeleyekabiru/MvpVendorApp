using MvpVendingMachineApp.Domain.Entities.Users;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Requests
{
    public class SignUpRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
