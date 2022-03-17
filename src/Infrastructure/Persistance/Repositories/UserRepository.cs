using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.Utilities;
using MvpVendingMachineApp.Domain.Entities.Users;
using MvpVendingMachineApp.Domain.IRepositories;
using MvpVendingMachineApp.Persistance.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository, IScopedDependency
    {
        public UserRepository(VendorWriteDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return UpdateAsync(user, cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginAttempt = System.DateTime.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName);
            if (exists)
                throw new VendorAppException("User already exist");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }
    }
}
