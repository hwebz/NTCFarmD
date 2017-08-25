using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using Gro.Infrastructure.Data.GrobarhetService;
using Gro.Core.DataModels.GrobarhetDtos;

namespace Gro.Infrastructure.Data.Repositories.Tests
{
    [TestClass]
    public class GrobarhetRepositoryTests
    {

        [TestMethod]
        public async Task GetGrobarhetAsyncTest_NullService()
        {
            var mockService = new Mock<IGrobarhetService>();
            mockService.Setup(w => w.GetGrobarhetAsync("", "")).Returns(Task.FromResult((GrobarhetResponse[])null));
            var service = mockService.Object;

            var repo = new GrobarhetRepository(service);
            var list = await repo.GetGrobarhetAsync("", "");

            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public async Task GetGrobarhetAsyncTest_LiveService()
        {
            var expected = new GrobarhetResponse[1] { new GrobarhetResponse { } };

            var mockService = new Mock<IGrobarhetService>();
            mockService.Setup(w => w.GetGrobarhetAsync("", "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new GrobarhetRepository(service);
            var actual = await repo.GetGrobarhetAsync("", "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }
    }
}
