using AutoMapper;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Application.Users.Command.Response;
using System.Net;
using MvpVendingMachineApp.Application.Users.Query;
using System.Collections.Generic;
using MvpVendingMachineApp.Application.Users.Response;
using MvpVendingMachineApp.Common.General.Constants;
using MvpVendingMachineApp.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace MvpVendingMachineApp.Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController : BaseControllerV1
    {
        public UserController(ILogger<UserController> logger,
                              IMediator mediator,
                              IMapper mapper)
            : base(logger, mediator, mapper)
        { }
        /// <summary>
        /// Craate a new user 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ApiResult<bool>> SingUpAsync([FromBody] SignUpRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SignUpRequest, CreateUserCommand>(request);

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<bool>(result, 201);
        }
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("login")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<LoginRequest, LoginCommand>(request);

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<LoginResponse>(result);
        }

        /// <summary>
        /// Update user credential
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.OK)]
        [HttpPut("update")]
        [Authorize]
        public virtual async Task<ApiResult<Unit>> UpdateAsync([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateUserRequest, UpdateUserCommand>(request);
            command.Id = User.GetUserId();
            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<Unit>(result);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.NotFound)]
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public virtual async Task<ApiResult<Unit>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand { UserId = id };

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<Unit>(result);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), (int)HttpStatusCode.OK)]
        [HttpGet("{id}")]
        [Authorize]
        public virtual async Task<ApiResult<GetUserByIdQueryModel>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var command = new GetUserbyIdQuery { UserId = id };

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<GetUserByIdQueryModel>(result);
        }

        /// <summary>
        /// Get all user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GetAllUserQueryModel>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Authorize]
        public virtual async Task<ApiResult<IEnumerable<GetAllUserQueryModel>>> GetAsync([FromQuery] GetAllUserQuery query, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(query, cancellationToken);
            return new ApiResult<IEnumerable<GetAllUserQueryModel>>(result);
        }


        /// <summary>
        /// deposit fund
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<DepositFundResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("deposit")]
        [Authorize(Policy = Policy.RequireBuyer)]
        public virtual async Task<ApiResult<DepositFundResponse>> DepositFund([FromBody] DepositFundRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<DepositFundRequest, DepositFundCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<DepositFundResponse>(result);
        }

        /// <summary>
        /// buy product
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<UserBuyProductResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("buy")]
        [Authorize(Policy = Policy.RequireBuyer)]
        public virtual async Task<ApiResult<UserBuyProductResponse>> BuyProduct([FromBody] UserBuyProductRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UserBuyProductRequest, UserBuyProductCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<UserBuyProductResponse>(result);
        }
        /// <summary>
        /// Reset fund
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Unit>), (int)HttpStatusCode.OK)]
        [HttpPost("reset")]
        [Authorize(Policy = Policy.RequireBuyer)]
        public virtual async Task<ApiResult<Unit>> Reset(CancellationToken cancellationToken)
        {
            var command = new ResetDepositCommand { UserId = User.GetUserId() };
            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<Unit>(result);
        }

    }
}
