using MvpVendingMachineApp.Domain.Entities.Users;

namespace MvpVendingMachineApp.Application.Users.Query
{
    public class GetUserByIdQueryModel
    {
        public GetUserByIdQueryModel()
        {

        }
        public GetUserByIdQueryModel(User user)
        {
            UserName = user.UserName;
            Deposit = user.Deposit;
        }
        public string UserName { get; }
        public decimal Deposit { get; }
    }
}
