using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvpVendingMachineApp.Application.Products.Command;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Persistance.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {

        private readonly VendorWriteDbContext dbContext;
        private readonly ILogger<UpdateProductCommandHandler> logger;
        public DeleteProductCommandHandler(VendorWriteDbContext dbContext, ILogger<UpdateProductCommandHandler> logger)
        {

            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().FirstOrDefaultAsync(a => a.SellerId == request.SellerId && a.Id == request.ProductId);

            if (existingProduct == null)
            {
                throw new VendorAppException(ApiResultStatusCode.NotFound, "product not found");
            }

            dbContext.Set<Product>().Remove(existingProduct);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Product Deleted {existingProduct.Id}");

            return Unit.Value;
        }
    }
}
