using MvpVendingMachineApp.Common.Exceptions;
using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Products.Command;

namespace MvpVendingMachineApp.Persistance.CommandHandlers.Products
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly VendorWriteDbContext dbContext;
        private readonly ILogger<AddProductCommandHandler> logger;

        public AddProductCommandHandler(VendorWriteDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().FirstOrDefaultAsync(a => a.ProductName == request.Name);

            if (existingProduct != null)
            {
                throw new ExistingRecordException("This Product has been added");
            }

            var product = new Product
            {
                ProductName = request.Name,
                Cost = request.Cost,
                AmountAvailable = request.AmountAvailable,
                SellerId = request.SellerId,
            };

            await dbContext.Set<Product>().AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product Inserted", product);

            return product.Id;
        }
    }
}
