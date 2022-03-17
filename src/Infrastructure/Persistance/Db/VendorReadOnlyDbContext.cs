using Microsoft.EntityFrameworkCore;

namespace MvpVendingMachineApp.Persistance.Db
{
    public class VendorReadOnlyDbContext : AppDbContext
    {
        public VendorReadOnlyDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
