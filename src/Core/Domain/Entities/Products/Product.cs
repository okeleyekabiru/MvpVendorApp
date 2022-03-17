namespace MvpVendingMachineApp.Domain.Entities.Products
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public int SellerId { get; set; }

        public decimal Cost { get; set; }
        public int AmountAvailable { get; set; }
    }
}
