namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Refresh_token { get; set; }
    }

}
