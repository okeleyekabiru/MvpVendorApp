namespace MvpVendingMachineApp.Application.Users.Command.Response
{
    public class LoginResponse
    {
        public readonly static LoginResponse Empty = new LoginResponse();

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
