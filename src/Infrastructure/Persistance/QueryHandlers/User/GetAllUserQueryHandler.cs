using MediatR;
using Microsoft.EntityFrameworkCore;
using MvpVendingMachineApp.Application.Users.Query;
using MvpVendingMachineApp.Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.QueryHandlers.User
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<GetAllUserQueryModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<GetAllUserQueryModel>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.TableNoTracking.Select(e => new GetAllUserQueryModel
            {
                Id = e.Id,
                UserName = e.UserName,
            }).ToListAsync();
        }
    }
}
