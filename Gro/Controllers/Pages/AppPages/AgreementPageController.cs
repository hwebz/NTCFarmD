using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.AgreementDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.AppPages.Agreement;
using Gro.Helpers;

namespace Gro.Controllers.Pages.AppPages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class AgreementPageController : SiteControllerBase<AgreementPage>
    {
        private readonly IAgreementRepository _agreementRepository;

        public AgreementPageController(
            IAgreementRepository agreementRepository,
            IUserManagementService usersManagementService,
            TicketProvider ticketProvider,
            IUserManagementService userManager) : base(userManager)
        {
            _agreementRepository = agreementRepository;
        }

        public ActionResult Index(AgreementPage currentPage)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (string.IsNullOrWhiteSpace(supplier?.CustomerNo))
            {
                return View("Index", new AgreementPageViewModel(currentPage)
                {
                    ListAgreementsByYears = new Agreement[0].GroupBy(a => 0),
                    ListSeedAgreementsByYears = new SeedAssurance[0].GroupBy(a => 0),
                    ListFarmingAgreements = new Agreement[0].GroupBy(a => 0),
                    ListDryAgreements = new List<DryAgreement>()
                });
            }

            var model = new AgreementPageViewModel(currentPage)
            {
                ListAgreementsByYears = _agreementRepository.GetAgreementsListByYears(supplier.CustomerNo),
                ListSeedAgreementsByYears = _agreementRepository.GetSeedAgreementsByYears(supplier.CustomerNo),
                ListFarmingAgreements = _agreementRepository.GetFarmingAgreementsByYears(supplier.CustomerNo),
                ListDryAgreements = _agreementRepository.GetDryAgreements(supplier.CustomerNo)
            };

            return View("Index", model);
        }


        [Route("api/agreement/price-hedging/{agreementNumber}")]
        public async Task<ActionResult> GetPriceHedging(string agreementNumber)
        {
            int agreementN;
            if (!int.TryParse(agreementNumber, out agreementN)) return new HttpStatusCodeResult(400);

            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (string.IsNullOrWhiteSpace(supplier.CustomerNo)) return HttpNotFound();

            var listPriceHedgings = await _agreementRepository.GetPriceHedgingListAsync(supplier.CustomerNo, agreementNumber);
            return PartialView("Dialogs/PriceHedgingBody", listPriceHedgings);
        }

        [Route("api/agreement/farmsample/{agreementNumber}")]
        public async Task<ActionResult> GetFarmSamples(string agreementNumber)
        {
            if (string.IsNullOrWhiteSpace(agreementNumber)) return new HttpStatusCodeResult(400);

            var supplier = UserManager.GetActiveCustomer(HttpContext);
            if (string.IsNullOrWhiteSpace(supplier.CustomerNo)) return HttpNotFound();

            var listPriceHedgings = await _agreementRepository.GetFarmSamplesListAsync(supplier.CustomerNo, agreementNumber);
            var groupOfAnalysis = listPriceHedgings
                .Select(x => new FarmSample { Analyskod = x.Analyskod, Resultat = x.Resultat, ProvNr = x.ProvNr })
                .Distinct()
                .ToList();

            // list all ProvNr
            var listProvNrs = groupOfAnalysis.Select(x => x.ProvNr).Distinct().OrderBy(x => x);

            var listItemByKodAndProvNr = groupOfAnalysis.GroupBy(x => new { x.Analyskod, x.ProvNr })
                .Select(y => new FarmSample
                {
                    Analyskod = y.Key.Analyskod,
                    ProvNr = y.Key.ProvNr,
                    Resultat = y.Select(a => a.Resultat).FirstOrDefault()
                }).OrderBy(x => x.Analyskod).ThenBy(x => x.ProvNr).ToList();

            var dictResultByAnalyskod = new Dictionary<string, List<string>>();
            foreach (var item in listItemByKodAndProvNr)
            {
                dictResultByAnalyskod.AddToDictionary(item.Analyskod, item.Resultat);
            }

            var viewModel = new AgreementAnalyzeViewModel
            {
                AgreementNumber = agreementNumber,
                AnalysisProvNrs = listProvNrs,
                DictResultByAnalyskod = dictResultByAnalyskod
            };
            return PartialView("Dialogs/AnalyzeBody", viewModel);
        }
    }
}
