using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Products.Command;
using MvpVendingMachineApp.Common;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly VendorWriteDbContext dbContext;
        private readonly ILogger<AddProductCommandHandler> logger;

        public UpdateProductCommandHandler(VendorWriteDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().FirstOrDefaultAsync(a => a.SellerId == request.SellerId && a.Id == request.ProductId);

            if (existingProduct == null)
            {
                throw new VendorAppException(ApiResultStatusCode.NotFound, "product not found");
            }

            existingProduct.AmountAvailable = request.AmountAvailable ?? existingProduct.AmountAvailable;
            existingProduct.Cost = request.Cost ?? existingProduct.Cost;
            existingProduct.ProductName = request.ProductName ?? existingProduct.ProductName;

            dbContext.Set<Product>().Update(existingProduct);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Product updated {existingProduct.Id}");

            return Unit.Value;
        }
    }
}
