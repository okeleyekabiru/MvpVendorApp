using System;

namespace MvpVendingMachineApp.Domain.Entities.Users
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public decimal Deposit { get; set; }
        public Role Role { get; set; }

        public DateTime LastLoginAttempt { get; set; }

    }

    public enum Role
    {
        Seller = 1,
        Buyer = 2,
    }

}
