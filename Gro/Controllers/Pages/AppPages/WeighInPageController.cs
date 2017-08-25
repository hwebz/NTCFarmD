using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.Interfaces;
using Gro.Core.DataModels.WeighInDtos;
using Gro.ViewModels.Pages.AppPages.WeighIn;
using Gro.Infrastructure.Data;
using System.Linq;
using System.Collections.Generic;
using Gro.Core.DataModels.AgreementDtos;
using System;
using Gro.Business.Services.Users;
using Gro.Core.DataModels.Security;
using System.Web;

namespace Gro.Controllers.Pages.AppPages
{
    [TemplateDescriptor(Inherited = true)]
    [CustomerRole]
    public class WeighInPageController : SiteControllerBase<WeighInPage>
    {
        private readonly IWeighInRepository _weighInRepository;
        private readonly IAgreementRepository _agreementRepository;
        private readonly string _ticket;

        public WeighInPageController(
            IWeighInRepository weighInRepo,
            IAgreementRepository agreementRepo,
            IUserManagementService userManager,
            TicketProvider ticketProvider) : base(userManager)
        {
            _weighInRepository = weighInRepo;
            _agreementRepository = agreementRepo;
            _ticket = ticketProvider.GetTicket();
        }

        private string GetActiveCustomerNumber(UserCore siteUser)
            => siteUser == null ? null : UserManager.GetActiveCustomer(HttpContext)?.CustomerNo;

        /// <summary>
        /// Count the sum of agreements and contracts spontaneously and merge them into a table.
        /// </summary>
        private async Task<IEnumerable<AgreementSumViewModel>> GetSortedAgreementSum(int year, string customerNumber)
        {
            var weighInSumAgreements = await _weighInRepository.GetWeighInSumAgreementAsync(year.ToString(), customerNumber, _ticket) ??
                new WeighInSumAgreementDto[0];
            var spontaneouslyAgreementQuery = (from t in weighInSumAgreements
                                               where t.Skordear == year.ToString()
                                               select new
                                               {
                                                   Sort = t.Artikelnamn,
                                                   HarvestYear = year,
                                                   Quantity = 0,
                                                   QuantitySpont = t.Summa
                                               }).ToList();

            var agreements = await _agreementRepository.GetAgreementsListAsync(customerNumber) ?? new Agreement[0];
            var thisYearAgreements = agreements.Where(a => a.HarvestYear == year);

            // Do summation of quantitatives by grouping by sort, harvest year
            var agreementQuery = (from t in thisYearAgreements
                                  group t by new { t.Sort, t.HarvestYear }
                                  into grp
                                  select new
                                  {
                                      grp.Key.Sort,
                                      grp.Key.HarvestYear,
                                      Quantity = grp.Sum(t => t.Weighed),
                                      QuantitySpont = 0
                                  }).ToList();

            var leftAgreementSumList =
                from aq in agreementQuery
                join saq in spontaneouslyAgreementQuery on aq.Sort equals saq.Sort into lq
                from saq in lq.DefaultIfEmpty()
                select new AgreementSumViewModel
                {
                    Sort = aq?.Sort,
                    Quantity = aq?.Quantity ?? 0,
                    QuantitySpont = saq?.QuantitySpont ?? 0
                };

            var rightAgreementSumList =
                from saq in spontaneouslyAgreementQuery
                join aq in agreementQuery on saq.Sort equals aq.Sort into lq
                from aq in lq.DefaultIfEmpty()
                select new AgreementSumViewModel
                {
                    Sort = saq?.Sort,
                    Quantity = aq?.Quantity ?? 0,
                    QuantitySpont = saq?.QuantitySpont ?? 0
                };

            var agreementSumList = leftAgreementSumList
                .Union(rightAgreementSumList)
                .Distinct(new AgreementSumComparer())
                .OrderBy(x => x.Sort);
            return agreementSumList;
        }

        public async Task<ActionResult> Index(WeighInPage currentPage)
        {
            int year;
            var yearQuery = Request.QueryString["year"];
            if (string.IsNullOrWhiteSpace(yearQuery) || !int.TryParse(yearQuery, out year))
            {
                year = DateTime.Now.Month < 7 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
            }

            var customerNumber = GetActiveCustomerNumber(SiteUser);
            if (string.IsNullOrWhiteSpace(customerNumber))
            {
                return View("Index", new WeighInPageViewModel(currentPage)
                {
                    WeighIns = new WeighIn[0],
                    AgreementSums = new AgreementSumViewModel[0],
                    CurrentYear = year
                });
            }

            var weighInList = await _weighInRepository.GetWeighInListAsync(customerNumber, year, _ticket) ?? new WeighIn[0];
            var sortedAgreementSum = await GetSortedAgreementSum(year, customerNumber);

            return View("Index", new WeighInPageViewModel(currentPage)
            {
                WeighIns = weighInList.OrderByDescending(w => w.Date),
                AgreementSums = sortedAgreementSum,
                CurrentYear = year
            });
        }


        [Route("api/weigh-in/analyze/{deliveryNumber}")]
        public async Task<ActionResult> GetAnalyzeList(string deliveryNumber, string date, string sort)
        {
            int deliveryId;
            if (!int.TryParse(deliveryNumber, out deliveryId)) return new HttpStatusCodeResult(400);

            var customerNumber = GetActiveCustomerNumber(SiteUser);
            if (string.IsNullOrWhiteSpace(customerNumber)) return HttpNotFound();

            var analyzeList = await _weighInRepository.GetAnalyzeListAsync(customerNumber, deliveryId, _ticket) ?? new AnalyzeList[0];

            ViewData["deliveryNumber"] = deliveryNumber;
            ViewData["customerNumber"] = customerNumber;
            ViewData["date"] = HttpUtility.UrlDecode(date);
            ViewData["sort"] = HttpUtility.UrlDecode(sort);

            return PartialView("AnalyzeList", analyzeList);
        }


        [Route("api/weigh-in/more-info/{deliveryNumber}")]
        public async Task<ActionResult> GetExtendedInfo(string deliveryNumber, string date, string sort)
        {
            int deliveryId;
            if (!int.TryParse(deliveryNumber, out deliveryId)) return new HttpStatusCodeResult(400);

            var customerNumber = GetActiveCustomerNumber(SiteUser);
            if (string.IsNullOrWhiteSpace(customerNumber)) return HttpNotFound();

            var extendedInfo = await _weighInRepository.GetMoreInfoAsync(customerNumber, deliveryId, _ticket) ?? new WeighInExtended[0];

            ViewData["deliveryNumber"] = deliveryNumber;
            ViewData["date"] = HttpUtility.UrlDecode(date);
            ViewData["sort"] = HttpUtility.UrlDecode(sort);

            return PartialView("ExtendedInfo", extendedInfo);
        }
    }
}
