namespace MvpVendingMachineApp.Application.Products.Query
{
    public class ProductQueryModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int AmountAvailable { get; set; }
    }
}
