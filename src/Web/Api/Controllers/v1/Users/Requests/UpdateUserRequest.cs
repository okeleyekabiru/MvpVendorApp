namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Requests
{
    public class UpdateUserRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
