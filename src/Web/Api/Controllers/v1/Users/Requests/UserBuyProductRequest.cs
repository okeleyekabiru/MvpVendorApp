namespace MvpVendingMachineApp.Api.Controllers.v1.Users.Requests
{
    public class UserBuyProductRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
