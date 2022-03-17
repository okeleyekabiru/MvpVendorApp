using MediatR;

namespace MvpVendingMachineApp.Application.Products.Command
{
    public class AddProductCommand : IRequest<int>
    {
        public int SellerId { get; set; }
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int AmountAvailable { get; set; }
    }
}
