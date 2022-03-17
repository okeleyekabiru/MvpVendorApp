namespace MvpVendingMachineApp.Api.Controllers.v1.Products.Requests
{
    public class UpdateProductRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal? Cost { get; set; }

        public int? AmountAvailable { get; set; }
    }
}
