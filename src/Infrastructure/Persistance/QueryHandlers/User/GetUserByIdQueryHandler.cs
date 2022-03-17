using MediatR;
using MvpVendingMachineApp.Application.Users.Query;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.QueryHandlers.User
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserbyIdQuery, GetUserByIdQueryModel>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<GetUserByIdQueryModel> Handle(GetUserbyIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);

            if (user == null)
                throw new VendorAppException(ApiResultStatusCode.NotFound, "user not found");


            return Task.FromResult(new GetUserByIdQueryModel(user));

        }
    }
}
