using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MvpVendingMachineApp.Api;
using MvpVendingMachineApp.Common.Utilities;
using MvpVendingMachineApp.Domain.Entities.Products;
using MvpVendingMachineApp.Domain.Entities.Users;
using MvpVendingMachineApp.IntegrationTest;
using NUnit.Framework;
using static MvpVendingMachineApp.IntegrationTest.Testing;
namespace MvpVendingMachineApp.IntegrationTest
{

    public class BaseControllerTest : WebApplicationFactory<Startup>
    {
        protected const string UserName = "bayoamzat20@gmail.com";
        protected const string Password = "Pass123*";
        private APIWebApplicationFactory _factory;
        protected HttpClient Client;
        private User _user;

        [OneTimeSetUp]
        public void StartUp()
        {
            _factory = new APIWebApplicationFactory();
            Client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Client.Dispose();
            _factory.Dispose();
        }

        protected async Task<User> CreateUser(Role role, decimal deposit = 0)
        {
            var users = await GetAllAsync<User>();
            await RemoveRange(users);
            var user = new User
            {
                Deposit = deposit,
                Role = role,
                PasswordHash = SecurityHelper.GetSha256Hash(Password),
                UserName = UserName
            };

            await AddAsync(user);
            _user = user;
            return user;
        }

        protected async Task RemoveUser()
        {
            //remove test user
            await Remove(_user);
        }

        protected async Task<Product> AddProduct(int amountAvaliable, decimal price)
        {
            var products = await GetAllAsync<Product>();
            await RemoveRange(products);
            var product = new Product
            {
                AmountAvailable = amountAvaliable,
                Cost = price,
                ProductName = "Cherry",
                SellerId = 1
            };
            await AddAsync(product);
            return product;
        }
    }
}