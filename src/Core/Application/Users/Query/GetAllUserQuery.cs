using MediatR;
using System.Collections.Generic;

namespace MvpVendingMachineApp.Application.Users.Query
{
    public class GetAllUserQuery : IRequest<IEnumerable<GetAllUserQueryModel>>
    {

    }
}
