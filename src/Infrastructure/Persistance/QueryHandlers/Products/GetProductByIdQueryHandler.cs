using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Products.Query;

namespace MvpVendingMachineApp.Persistance.QueryHandlers.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductQueryModel>
    {
        private readonly VendorReadOnlyDbContext dbContext;

        public GetProductByIdQueryHandler(VendorReadOnlyDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ProductQueryModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().Where(a => a.Id == request.ProductId).Select(a =>
               new ProductQueryModel
               {
                   Name = a.ProductName,
                   Cost = a.Cost,
                   AmountAvailable = a.AmountAvailable,
                   ProductId = a.Id
               }).FirstOrDefaultAsync();

            return existingProduct;
        }
    }
}