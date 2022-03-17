using MediatR;

namespace MvpVendingMachineApp.Application.Products.Query
{
    public class GetProductByIdQuery : IRequest<ProductQueryModel>
    {
        public int ProductId { get; set; }
    }
}