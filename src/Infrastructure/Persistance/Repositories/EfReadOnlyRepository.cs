using MvpVendingMachineApp.Domain.Entities;
using MvpVendingMachineApp.Domain.IRepositories;
using MvpVendingMachineApp.Persistance.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Persistance.Repositories
{
    public class EfReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly VendorReadOnlyDbContext DbContext;

        public DbSet<TEntity> Entities { get; }

        public virtual IQueryable<TEntity> Table => Entities;

        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public EfReadOnlyRepository(VendorReadOnlyDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>();
        }

        public virtual ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return Entities.FindAsync(ids, cancellationToken);
        }

        public virtual TEntity GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }
    }
}