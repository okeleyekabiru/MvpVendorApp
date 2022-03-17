using MediatR;
using MvpVendingMachineApp.Application.Products.Query;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Application.Users.Response;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.Utilities;
using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Domain.IRepositories;
using MvpVendingMachineApp.Persistance.Db;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Users
{
    public class UserBuyProductCommandHandler : IRequestHandler<UserBuyProductCommand, UserBuyProductResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly VendorWriteDbContext dbContext;

        public UserBuyProductCommandHandler(IMediator mediator, IUserRepository userRepository, VendorWriteDbContext dbContext)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            this.dbContext = dbContext;
        }
        public async Task<UserBuyProductResponse> Handle(UserBuyProductCommand request, CancellationToken cancellationToken)
        {
            var product = dbContext.Set<Product>().Find(request.ProductId);
            if (product == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "Product not found");

            var user = await _userRepository.GetByIdAsync(cancellationToken, request.UserId);
            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "User not found");

            var userOrderCost = request.Quantity * product.Cost;
            if (user.Deposit < userOrderCost)
                throw new VendorAppException(ApiResultStatusCode.BadRequest, "Insufficient balance to purchase items");
            if (product.AmountAvailable == 0)
                throw new VendorAppException(ApiResultStatusCode.BadRequest, "Item out of stock");
            if (request.Quantity > product.AmountAvailable)
                throw new VendorAppException(ApiResultStatusCode.BadRequest, "Order request requested is more than item in stock");

            user.Deposit -= userOrderCost;
            product.AmountAvailable -= request.Quantity;

            _userRepository.Update(user, true);
            dbContext.Set<Product>().Update(product);
            dbContext.SaveChanges();

            return new UserBuyProductResponse { Change = BusinessLogic.CalculateChange(user.Deposit), ProductName = product.ProductName, TotalAmount = userOrderCost };
        }
    }
}
