namespace MvpVendingMachineApp.Application.Users.Response
{
    public class UserBuyProductResponse
    {
        public decimal TotalAmount { get; set; }
        public string ProductName { get; set; }
        public decimal[] Change { get; set; }
    }
}
