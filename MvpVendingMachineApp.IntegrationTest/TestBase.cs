using System.Threading.Tasks;
using MvpVendingMachineApp.IntegrationTest;
using NUnit.Framework;

namespace TMvpVendingMachineApp.IntegrationTest
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task SetUp()
        {
            await ResetDbState();
        }
    }
}