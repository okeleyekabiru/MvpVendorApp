using Newtonsoft.Json;

namespace MvpVendingMachineApp.Api.Controllers.v1.Products.Requests
{
    public class AddProductRequest
    {
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int AmountAvailable { get; set; }
    }
}
