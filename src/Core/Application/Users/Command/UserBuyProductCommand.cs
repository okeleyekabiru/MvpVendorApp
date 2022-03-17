using MediatR;
using MvpVendingMachineApp.Application.Users.Response;

namespace MvpVendingMachineApp.Application.Users.Command
{
    public class UserBuyProductCommand : IRequest<UserBuyProductResponse>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
    }
}
