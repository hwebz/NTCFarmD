using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using Gro.Core.DataModels.WeighInDtos;
using Gro.Infrastructure.Data.WeighInService;

namespace Gro.Infrastructure.Data.Repositories.Tests
{
    [TestClass]
    public class WeighInRepositoryTests
    {

        [TestMethod]
        public async Task GetAnalyzeListAsyncTest_NullService()
        {
            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetAnalyzeListAsync("", 0, "")).Returns(Task.FromResult((AnalyzeList[])null));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var list = await repo.GetAnalyzeListAsync("", 0, "");

            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public async Task GetAnalyzeListAsyncTest_LiveService()
        {
            var expected = new AnalyzeList[1] { new AnalyzeList { } };

            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetAnalyzeListAsync("", 0, "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var actual = await repo.GetAnalyzeListAsync("", 0, "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public async Task GetMoreInfoAsyncTest_NullService()
        {
            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetMoreInfoAsync("", 0, "")).Returns(Task.FromResult((WeighInExtended[])null));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var list = await repo.GetMoreInfoAsync("", 0, "");

            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public async Task GetMoreInfoAsyncTest_LiveService()
        {
            var expected = new WeighInExtended[1] { new WeighInExtended { } };

            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetMoreInfoAsync("", 0, "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var actual = await repo.GetMoreInfoAsync("", 0, "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public async Task GetOverViewListAsyncTest_NullService()
        {
            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetOverViewListAsync("", 0, "")).Returns(Task.FromResult((Overview[])null));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var list = await repo.GetOverViewListAsync("", 0, "");

            Assert.AreEqual(0, list.Length);
        }


        [TestMethod]
        public async Task GetOverViewListAsyncTest_LiveService()
        {
            var expected = new Overview[1] { new Overview { } };

            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetOverViewListAsync("", 0, "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var actual = await repo.GetOverViewListAsync("", 0, "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public async Task GetWeighInListAsyncTest_NullService()
        {
            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetWeighInListAsync("", 0, "")).Returns(Task.FromResult((WeighIn[])null));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var list = await repo.GetWeighInListAsync("", 0, "");

            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public async Task GetWeighInListAsyncTest_LiveService()
        {
            var expected = new WeighIn[1] { new WeighIn { } };

            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetWeighInListAsync("", 0, "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var actual = await repo.GetWeighInListAsync("", 0, "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        public async Task GetWeighInSumAgreementAsyncTest_NullService()
        {
            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetWeighInSumAgreementAsync("", "", "")).Returns(Task.FromResult((WeighInSumAgreementDto[])null));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var list = await repo.GetWeighInSumAgreementAsync("", "", "");

            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public async Task GetWeighInSumAgreementAsyncTest_LiveService()
        {
            var expected = new WeighInSumAgreementDto[1] { new WeighInSumAgreementDto { } };

            var mockService = new Mock<IWeighInService>();
            mockService.Setup(w => w.GetWeighInSumAgreementAsync("", "", "")).Returns(Task.FromResult(expected));
            var service = mockService.Object;

            var repo = new WeighInRepository(service);
            var actual = await repo.GetWeighInSumAgreementAsync("", "", "");

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }
    }
}
