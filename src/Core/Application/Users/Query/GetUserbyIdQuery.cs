using MediatR;

namespace MvpVendingMachineApp.Application.Users.Query
{
    public class GetUserbyIdQuery : IRequest<GetUserByIdQueryModel>
    {
        public int UserId { get; set; }
    }
}
