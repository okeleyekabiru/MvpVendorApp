using Microsoft.EntityFrameworkCore;

namespace MvpVendingMachineApp.Persistance.Db
{
    public class VendorWriteDbContext : AppDbContext
    {
        public VendorWriteDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}