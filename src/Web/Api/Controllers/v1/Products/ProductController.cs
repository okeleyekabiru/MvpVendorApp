using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Products.Query;
using Microsoft.AspNetCore.Authorization;
using MvpVendingMachineApp.Common.General.Constants;
using MvpVendingMachineApp.Api.Controllers.v1.Products.Requests;
using MvpVendingMachineApp.Application.Products.Command;
using MvpVendingMachineApp.Common.Utilities;
using System.Net;

namespace MvpVendingMachineApp.Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    public class ProductController : BaseControllerV1
    {
        public ProductController(ILogger<ProductController> logger,
                                 IMediator mediator,
                                 IMapper mapper)
            : base(logger, mediator, mapper)
        { }

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<ProductQueryModel>> GetByIdAsync([FromQuery] int productId)
        {
            var result = await _mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [Authorize(Policy = Policy.RequireSeller)]
        [HttpPost]
        public async Task<ApiResult<int>> AddProduct([FromBody] AddProductRequest command)
        {
            var result = await _mediator.Send(new AddProductCommand { Name = command.Name, AmountAvailable = command.AmountAvailable, Cost = command.Cost, SellerId = User.GetUserId() });
            return new ApiResult<int>(result);
        }
        /// <summary>
        /// Update seller product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = Policy.RequireSeller)]
        [HttpPut("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.NotFound)]
        public async Task<ApiResult<Unit>> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            var product = _mapper.Map<UpdateProductRequest, UpdateProductCommand>(request);
            product.SellerId = User.GetUserId();
            var result = await _mediator.Send(product);
            return new ApiResult<Unit>(result);
        }

        /// <summary>
        /// Update seller product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = Policy.RequireSeller)]
        [HttpPut("delete")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.NotFound)]
        public async Task<ApiResult<Unit>> DeleteProduct([FromBody] DeleteProductRequest request)
        {
            var product = _mapper.Map<DeleteProductRequest, DeleteProductCommand>(request);
            product.SellerId = User.GetUserId();
            var result = await _mediator.Send(product);
            return new ApiResult<Unit>(result);
        }

    }
}