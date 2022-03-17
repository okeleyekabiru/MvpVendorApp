using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Common.General.Constants
{
    public class Policy
    {
        public const string RequireSeller = nameof(RequireSeller);
        public const string RequireBuyer = nameof(RequireBuyer);
    }

    public class Role
    {
        public const string Seller = nameof(Seller);
        public const string Buyer = nameof(Buyer);
    }
}
