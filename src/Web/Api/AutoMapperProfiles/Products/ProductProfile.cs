using AutoMapper;
using MvpVendingMachineApp.Api.Controllers.v1.Products.Requests;
using MvpVendingMachineApp.Application.Products.Command;

namespace MvpVendingMachineApp.Api.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequest, AddProductCommand>();
            CreateMap<UpdateProductRequest, UpdateProductCommand>();
            CreateMap<DeleteProductRequest, DeleteProductCommand>();
        }
    }
}
