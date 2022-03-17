using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using MvpVendingMachineApp.Api;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using MvpVendingMachineApp.Application.Users.Command.Response;
using MvpVendingMachineApp.Application.Users.Query;
using MvpVendingMachineApp.Application.Users.Response;
using MvpVendingMachineApp.Domain.Entities.Users;
using Newtonsoft.Json;
using NUnit.Framework;
using static MvpVendingMachineApp.IntegrationTest.Testing;
namespace MvpVendingMachineApp.IntegrationTest
{
    [TestFixture]
    public class UserControllerTests : BaseControllerTest
    {
        [Test]
        public async Task LoginShouldReturnJwt()
        {
            await CreateUser(Domain.Entities.Users.Role.Buyer);

            var request = new LoginRequest
            {

                UserName = UserName,
                Password = Password,

            };

            var jsonString = JsonConvert.SerializeObject(request);
            Client.DefaultRequestHeaders.Clear();
            var response = await Client.PostAsync("/api/v1/User/login", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = JsonConvert.DeserializeObject<ApiResult<LoginResponse>>(content);
            token.Data.AccessToken.Should().NotBeNullOrEmpty();

            await RemoveUser();
        }
        [Test]
        public async Task ShoouldCreateUser()
        {

            var request = new SignUpRequest
            {

                UserName = UserName,
                Password = Password,
                Role = Domain.Entities.Users.Role.Buyer

            };

            Client.DefaultRequestHeaders.Clear();
            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/signup", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = JsonConvert.DeserializeObject<ApiResult<bool>>(content);
            token.Data.Should().Be(true);

            var users = await GetAllAsync<User>(e => e.UserName == UserName);

            users.Should().HaveCount(1);

            await Remove(users.First());


        }

        [Test]
        public async Task ShoouldUpdateUser()
        {
            var user = await CreateUser(Role.Buyer);
            var newPassword = "string12345%";

            var request = new UpdateUserRequest
            {

                OldPassword = Password,
                NewPassword = newPassword
            };

            var jsonString = JsonConvert.SerializeObject(request);
            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make update request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);

            var response = await Client.PutAsync("/api/v1/User/update", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonConvert.DeserializeObject<ApiResult<Unit>>(content);
            data.Data.Should().Be(Unit.Value);

            var authToken2 = await DoLogin(newPassword);
            authToken2.Should().NotBeNull();

            await RemoveUser();

        }

        [Test]
        public async Task ShoouldGetUser()
        {
            var user = await CreateUser(Role.Buyer);

            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make update request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);

            var response = await Client.GetAsync($"/api/v1/User/{user.Id}");

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonConvert.DeserializeObject<ApiResult<GetUserByIdQueryModel>>(content);
            data.Data.Should().NotBeNull();



            await RemoveUser();

        }

        [Test]
        public async Task ShoouldDeleteUser()
        {
            var user = await CreateUser(Role.Buyer);

            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make update request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);

            var response = await Client.DeleteAsync($"/api/v1/User/Delete/{user.Id}");

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonConvert.DeserializeObject<ApiResult<Unit>>(content);

            data.Data.Should().Be(Unit.Value);

            var users = await GetAllAsync<User>(e => e.UserName == UserName);

            users.Should().HaveCount(0);




        }


        [Test]
        public async Task Should_Deposit_Successfully()
        {
            var user = await CreateUser(Role.Buyer);

            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new DepositFundRequest { Amount = 10 };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/deposit", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonConvert.DeserializeObject<ApiResult<DepositFundResponse>>(content);

            data.Data.Deposit.Should().Be(10);
            data.Data.Balance.Should().Be(10);
            var authToken2 = await DoLogin();
            authToken.Should().NotBeNull();
            //make sencond request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken2);
            var request2 = new DepositFundRequest { Amount = 10 };

            var jsonString2 = JsonConvert.SerializeObject(request2);
            var response2 = await Client.PostAsync("/api/v1/User/deposit", new StringContent(jsonString2, Encoding.UTF8, "application/json"));
            var content2 = await response2.Content.ReadAsStringAsync();
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            var data2 = JsonConvert.DeserializeObject<ApiResult<DepositFundResponse>>(content2);
            data2.Data.Deposit.Should().Be(10);
            data2.Data.Balance.Should().Be(20);

        }

        [Test]
        public async Task Deposit_Should_Fail()
        {
            var user = await CreateUser(Role.Buyer);

            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new DepositFundRequest { Amount = 24 };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/deposit", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var data = JsonConvert.DeserializeObject<ApiResult<string>>(content);

            data.Data.Should().Contain("The specified condition was not met for 'Amount");

        }

        [Test]
        public async Task Deposit_Should_Fail_For_Seller()
        {
            var user = await CreateUser(Role.Seller);

            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new DepositFundRequest { Amount = 24 };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/deposit", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        [Test]
        public async Task Buy_Should_Fail_For_Seller_Buy()
        {
            var user = await CreateUser(Role.Seller);
            var product = AddProduct(30, 10);
            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new UserBuyProductRequest
            {
                ProductId = product.Id,
                Quantity = 10
            };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/buy", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        [Test]
        public async Task Buy_Should_Fail_For_insufficent_deposit_Buy()
        {
            var user = await CreateUser(Role.Buyer);
            var product = await AddProduct(30, 10);
            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new UserBuyProductRequest
            {
                ProductId = product.Id,
                Quantity = 10
            };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/buy", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<string>>(content);

            data.Errors.Should().NotBeEmpty();
            data.Errors.Should().Contain("Insufficient balance to purchase items");
        }

        [Test]
        public async Task Buy_Should_Be_Successfully()
        {
            var user = await CreateUser(Role.Buyer, 100);
            var product = await AddProduct(30, 10);
            //get token
            var authToken = await DoLogin();
            authToken.Should().NotBeNull();
            //make request
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
            var request = new UserBuyProductRequest
            {
                ProductId = product.Id,
                Quantity = 2
            };

            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/buy", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<UserBuyProductResponse>>(content);

            data.Data.Change.Length.Should().Be(3);
            data.Data.Change[0].Should().Be(50);
            data.Data.Change[1].Should().Be(20);
            data.Data.Change[2].Should().Be(10);
            data.Data.ProductName.Should().Be("Cherry");
        }
        private async Task<string> DoLogin(string password = null!)
        {
            var request = new LoginRequest
            {

                UserName = UserName,
                Password = password ?? Password,

            };
            Client.DefaultRequestHeaders.Clear();
            var jsonString = JsonConvert.SerializeObject(request);
            var response = await Client.PostAsync("/api/v1/User/login", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = JsonConvert.DeserializeObject<ApiResult<LoginResponse>>(content);

            return token!.Data.AccessToken;
        }
    }
}