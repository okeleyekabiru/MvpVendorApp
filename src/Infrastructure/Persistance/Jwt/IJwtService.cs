using MvpVendingMachineApp.Domain.Entities.Users;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}
