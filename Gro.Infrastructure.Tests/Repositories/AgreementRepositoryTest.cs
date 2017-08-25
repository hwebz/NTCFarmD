using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.DataModels.AgreementDtos;
using Gro.Infrastructure.Data.AgreementService;
using Gro.Infrastructure.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gro.Infrastructure.Tests.Repositories
{

    [TestClass]
    public class AgreementRepositoryTest
    {
        private readonly string supplier = "1000000";
        private readonly string ticket = string.Empty;

        private List<Agreement> ListAgreementsInYears => new List<Agreement>
        {
            new Agreement {HarvestYear = 2015},
            new Agreement {HarvestYear = 2016},
            new Agreement {HarvestYear = 2015}
        };

        [TestMethod]
        public void GetAgreementsListByYears_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(w => w.GetAgreementList(string.Empty, string.Empty)).Returns(new Agreement[0]);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var list = repo.GetAgreementsListByYears(string.Empty);
            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void GetAgreementsListByYears_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var listExpectedAgreement = ListAgreementsInYears;
            mockService.Setup(w => w.GetAgreementList(supplier, ticket)).Returns(listExpectedAgreement.ToArray);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var resultAsList = repo.GetAgreementsListByYears(supplier);
            var resultAsEnumerable = resultAsList as IGrouping<int, Agreement>[] ?? resultAsList.ToArray();

            Assert.AreEqual(2, resultAsEnumerable.Count());
            Assert.AreEqual(null, resultAsEnumerable.SingleOrDefault(x => x.Key == 2014));
            Assert.AreEqual(2, resultAsEnumerable.Single(x => x.Key == 2015).Count());
            Assert.AreEqual(1, resultAsEnumerable.Single(x => x.Key == 2016).Count());
        }

        [TestMethod]
        public void GetSeedAgreementsByYears_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(x => x.GetSeedAgreementList(supplier, ticket)).Returns(new SeedAssurance[0]);

            var service = mockService.Object;
            var repo = new AgreementRepository(service, null);
            var result = repo.GetSeedAgreementsByYears(supplier);
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public void GetSeedAgreementsByYears_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var listSeedAssuranceInYear = new List<SeedAssurance>()
            {
                new SeedAssurance{HarvestYear = 2015},
                new SeedAssurance{HarvestYear = 2015,},
                new SeedAssurance{HarvestYear   = 2016}
            };

            mockService.Setup(x => x.GetSeedAgreementList(supplier, ticket)).Returns(listSeedAssuranceInYear.ToArray());
            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var result = repo.GetSeedAgreementsByYears(supplier);
            var resultAsEnumerable = result as IGrouping<int, SeedAssurance>[] ?? result.ToArray();

            Assert.AreEqual(2, resultAsEnumerable.Count());
            Assert.AreEqual(null, resultAsEnumerable.SingleOrDefault(x => x.Key == 2014));
            Assert.AreEqual(2, resultAsEnumerable.Single(x => x.Key == 2015).Count());
            Assert.AreEqual(1, resultAsEnumerable.Single(x => x.Key == 2016).Count());
        }

        [TestMethod]
        public void GetFarmingAgreementsByYears_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(w => w.GetFarmingAgreementList(string.Empty, string.Empty)).Returns(new Agreement[0]);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var list = repo.GetFarmingAgreementsByYears(string.Empty);
            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void GetFarmingAgreementsByYears_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var listExpectedAgreement = ListAgreementsInYears;
            mockService.Setup(w => w.GetFarmingAgreementList(supplier, ticket)).Returns(listExpectedAgreement.ToArray);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var resultAsList = repo.GetFarmingAgreementsByYears(supplier);
            var resultAsEnumerable = resultAsList as IGrouping<int, Agreement>[] ?? resultAsList.ToArray();

            Assert.AreEqual(2, resultAsEnumerable.Count());
            Assert.AreEqual(null, resultAsEnumerable.SingleOrDefault(x => x.Key == 2014));
            Assert.AreEqual(2, resultAsEnumerable.Single(x => x.Key == 2015).Count());
            Assert.AreEqual(1, resultAsEnumerable.Single(x => x.Key == 2016).Count());
        }


        [TestMethod]
        public void GetDryAgreements_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(w => w.GetDryAgreementList(string.Empty, string.Empty)).Returns(new DryAgreement[0]);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var list = repo.GetFarmingAgreementsByYears(string.Empty);
            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void GetDryAgreements_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var listExpectedDryAgreements = new List<DryAgreement>()
            {
                new DryAgreement(), new DryAgreement()
            };
            mockService.Setup(w => w.GetDryAgreementList(supplier, ticket)).Returns(listExpectedDryAgreements.ToArray);

            var service = mockService.Object;

            var repo = new AgreementRepository(service, null);
            var resultAsList = repo.GetDryAgreements(supplier);

            Assert.AreEqual(2, resultAsList.Count());
        }

        [TestMethod]
        public async Task GetPriceHedgingListAsync_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(w => w.GetPriceHedgingAsync(string.Empty, string.Empty, string.Empty)).Returns(Task.FromResult((PriceHedging)null));

            var service = mockService.Object;
            var result = await (new AgreementRepository(service, null)).GetPriceHedgingListAsync(supplier, string.Empty);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public async Task GetPriceHedgingListAsync_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var listExpected = new PriceHedging[] { new PriceHedging() };
            mockService.Setup(w => w.GetPriceHedgingListAsync(supplier, string.Empty, ticket)).Returns(Task.FromResult(listExpected));

            var service = mockService.Object;
            var result = await (new AgreementRepository(service, null)).GetPriceHedgingListAsync(supplier, string.Empty);
            Assert.AreEqual(listExpected.Length, result.Length);
        }

        [TestMethod]
        public async Task GetFarmSamplesListAsyn_NullService()
        {
            var mockService = new Mock<IAgreementService>();
            mockService.Setup(w => w.GetFarmSampleListAsync(string.Empty, string.Empty, string.Empty)).Returns(Task.FromResult((FarmSample[])null));

            var service = mockService.Object;
            var repo = new AgreementRepository(service, null);
            var result = await repo.GetFarmSamplesListAsync(supplier, string.Empty);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public async Task GetFarmSamplesListAsyn_LiveService()
        {
            var mockService = new Mock<IAgreementService>();
            var expectedPriceHeding = new FarmSample[1] { new FarmSample { } };
            mockService.Setup(w => w.GetFarmSampleListAsync(supplier, string.Empty, ticket)).Returns(Task.FromResult(expectedPriceHeding));

            var service = mockService.Object;
            var repo = new AgreementRepository(service, null);
            var result = await repo.GetFarmSamplesListAsync(supplier, string.Empty);
            Assert.AreEqual(1, result.Length);
        }
    }
}