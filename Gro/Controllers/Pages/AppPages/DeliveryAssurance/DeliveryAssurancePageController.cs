using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Gro.Business.Services.Users;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.DeliveryAssuranceDtos;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.AppPages.DeliveryAssurance;

namespace Gro.Controllers.Pages.AppPages.DeliveryAssurance
{
    [TemplateDescriptor(Inherited = true)]
    public class DeliveryAssurancePageController : SiteControllerBase<DeliveryAssuranceListPage>
    {
        private const string DefaultWarehouse = "-1";
        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        private readonly IDeliveryAssuranceRepository _deliveryNoteRepository;

        public DeliveryAssurancePageController(
            IDeliveryAssuranceRepository deliveryNoteRepository,
            IUserManagementService userManager) : base(userManager)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
        }

        public async Task<ActionResult> Index(DeliveryAssuranceListPage currentPage, string cunr)
        {
            var supplier = UserManager.GetActiveCustomer(HttpContext);

            if (SiteUser == null || supplier == null)
            {
                TempData["NotLoggedIn"] = true;
                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceList.cshtml", new DeliveryAssuranceListViewModel(currentPage));
            }

            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            var deliveryAssuranceUrl = urlHelper.ContentUrl(currentPage.ContentLink);
            var bookDeliveryUrl = urlHelper.ContentUrl(currentPage.BookDeliveryPageUrl);

            var deliveryAssurances = await _deliveryNoteRepository.GetDeliveryAssurancesAsync(supplier?.CustomerNo);
            var model = new DeliveryAssuranceListViewModel(currentPage)
            {
                ListOfConfirmed = deliveryAssurances?.ListOfConfirmed?.Where(x => x != null).OrderBy(x => x.DeliveryDate)
                                    .Select(x => new DeliveryAssuranceListItem(x, deliveryAssuranceUrl, cunr)).ToList(),
                ListOfDelivered = deliveryAssurances?.ListOfDelivered?.Where(x => x != null).OrderBy(x => x.DeliveryDate)
                                    .Select(x => new DeliveryAssuranceListItem(x, deliveryAssuranceUrl, cunr)).ToList(),
                ListOfNotConfirmed = deliveryAssurances?.ListOfNotConfirmed?.Where(x => x != null).OrderBy(x => x.DeliveryDate)
                                    .Select(x => new DeliveryAssuranceListItem(x, deliveryAssuranceUrl, cunr)).ToList(),

                NotConfirmedCount = deliveryAssurances?.ListOfNotConfirmed?.Length ?? 0,
                ConfirmedCount = deliveryAssurances?.ListOfConfirmed?.Length ?? 0,
                DeliveredCount = deliveryAssurances?.ListOfDelivered?.Length ?? 0,

                IsInternal = SettingPage.IsInternal,
                IsShowCreateDelAssFromOtherLink = SettingPage.IsInternal || DeliveryAssuranceHelper.IsInHarvestPeriod(),
                IsShowCreateNewLink = SettingPage.IsInternal || !DeliveryAssuranceHelper.IsInStoragePeriod(),
                CreateNewUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{deliveryAssuranceUrl}Create", new Dictionary<string, string>
                {
                    {"cunr", cunr}
                }),
                BookDeliveryUrl = bookDeliveryUrl
            };

            return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceList.cshtml", model);
        }

        public async Task<ActionResult> Overview(DeliveryAssuranceListPage currentPage, string a, string l)
        {
            if (SiteUser == null)
            {
                return RedirectToAction("Index", new { node = currentPage.ContentLink });
            }

            var ioNumber = a;
            int lineNumber;

            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber))
            {
                var deliveryAssurance = await _deliveryNoteRepository.GetDeliveryAssuranceAsync(ioNumber, lineNumber);

                if (deliveryAssurance == null) return RedirectToAction("Index", new { node = currentPage.ContentLink });

                var model = await PopulateOverviewModel(currentPage, deliveryAssurance);

                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceOverview.cshtml", model);
            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        public async Task<ActionResult> Create(DeliveryAssuranceListPage currentPage, string a, string l, string cunr)
        {
            if (SiteUser == null)
            {
                return RedirectToAction("Index", new { node = currentPage.ContentLink });
            }

            var ioNumber = a;
            int lineNumber;

            // create from existed
            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber)
                && (SettingPage.IsInternal || DeliveryAssuranceHelper.IsInHarvestPeriod()))
            {
                var deliveryAssurance = await _deliveryNoteRepository.GetDeliveryAssuranceAsync(ioNumber, lineNumber);

                if (deliveryAssurance == null)
                    return RedirectToAction("Index", new { node = currentPage.ContentLink });

                var model = await CreateDeliveryAssuranceViewModel(currentPage, deliveryAssurance, DeliveryAssuranceAction.Create);
                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceForm.cshtml", model);
            }

            if(SettingPage.IsInternal || !DeliveryAssuranceHelper.IsInStoragePeriod())// create new one
            {
                var supplier = UserManager.GetActiveCustomer(HttpContext);
                var deliveryAssurance = await _deliveryNoteRepository.GetDefaultDeliveryAssuranceAsync(supplier?.CustomerNo);

                if (deliveryAssurance == null)
                    return RedirectToAction("Index", new { node = currentPage.ContentLink });

                deliveryAssurance.SupplierNumber = supplier?.CustomerNo;
                deliveryAssurance.Leveransdatum = DateTime.Now;
                //if (!deliveryAssurance.Gardshamtning && deliveryAssurance.Leveransvillkor == null)
                //{
                //    deliveryAssurance.Leveransvillkor = DeliveryAssuranceTermConditions.OwnTransport;
                //}

                var model = await CreateDeliveryAssuranceViewModel(currentPage, deliveryAssurance, DeliveryAssuranceAction.Create);

                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceForm.cshtml", model);

            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        public async Task<ActionResult> Change(DeliveryAssuranceListPage currentPage, string a, string l, string cunr)
        {
            if (SiteUser == null)
            {
                return RedirectToAction("Index", new { node = currentPage.ContentLink });
            }

            var ioNumber = a;
            int lineNumber;

            // create from existed
            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber))
            {
                var deliveryAssurance = await _deliveryNoteRepository.GetDeliveryAssuranceAsync(ioNumber, lineNumber);

                if (deliveryAssurance == null)
                    return RedirectToAction("Index", new { node = currentPage.ContentLink });

                if (!SettingPage.IsInternal && deliveryAssurance.Status > 35 ||
                        SettingPage.IsInternal && deliveryAssurance.Status >= 45)
                {
                    return RedirectToAction("Index", new { node = currentPage.ContentLink });
                }

                var model = await CreateDeliveryAssuranceViewModel(currentPage, deliveryAssurance, DeliveryAssuranceAction.Change);
                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceForm.cshtml", model);
            }
            return null;
        }

        public async Task<ActionResult> Approve(DeliveryAssuranceListPage currentPage, string a, string l, string cunr)
        {
            if (SiteUser == null)
            {
                return RedirectToAction("Index", new { node = currentPage.ContentLink });
            }

            var ioNumber = a;
            int lineNumber;

            if (TempData[nameof(MultiApprove)] != null)
            {
                Session[nameof(MultiApprove)] = TempData[nameof(MultiApprove)];
                TempData.Remove(nameof(MultiApprove));
            }
            else
            {
                Session.Remove(nameof(MultiApprove));
            }

            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber))
            {
                var deliveryAssurance = await _deliveryNoteRepository.GetDeliveryAssuranceAsync(ioNumber, lineNumber);

                if (deliveryAssurance == null)
                {
                    return RedirectToAction("Index", new { node = currentPage.ContentLink });
                }

                var model = await CreateDeliveryAssuranceViewModel(currentPage, deliveryAssurance, DeliveryAssuranceAction.Approve);

                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceForm.cshtml", model);
            }
            return null;
        }

        [HttpPost]
        public ActionResult MultiApprove(DeliveryAssuranceListPage currentPage, DeliveryAssuranceList[] deliveryAssurances)
        {
            var selectedDeliveryAssurances =
                deliveryAssurances.Where(x => !string.IsNullOrWhiteSpace(x.IONumber)).ToList();

            var count = selectedDeliveryAssurances.Count;
            if (count > 0)
            {
                var firstItem = selectedDeliveryAssurances.FirstOrDefault();
                if (firstItem != null)
                {
                    var result =
                        selectedDeliveryAssurances.Count(
                            x =>
                                x.DeliveryDate.Equals(firstItem.DeliveryDate) &&
                                x.Gardshamtning.Equals(firstItem.Gardshamtning) && x.Item.Equals(firstItem.Item));

                    if (result.Equals(count))
                    {
                        TempData[nameof(MultiApprove)] = selectedDeliveryAssurances;
                        return RedirectToAction("Approve", new { node = currentPage.ContentLink, a = firstItem.IONumber, l = firstItem.LineNumber.ToString(CultureInfo.InvariantCulture) });
                    }
                }
            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        [HttpPost]
        public async Task<ActionResult> Submit(DeliveryAssuranceListPage currentPage, DeliveryAssuranceDetail deliveryAssurance)
        {
            if (SiteUser == null)
            {
                return HttpNotFound();
            }

            var overviewModel = await PopulateOverviewModelFromDeliveryDetail(currentPage, deliveryAssurance);

            return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceOverview.cshtml", overviewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(DeliveryAssuranceListPage currentPage, 
            string action, string IONumber, string createNew, string noOfNew, string createPdf)
        {
            if (SiteUser == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(noOfNew)) noOfNew = "1";

            var sessionKey = $"{nameof(DeliveryAssuranceDetail)}-{action}-{IONumber}";
            var deliveryAssurance = Session[sessionKey] as DeliveryAssuranceDetail;
            if (deliveryAssurance == null) return RedirectToAction("Index", new { node = currentPage.ContentLink });

            var saveDeliveryAssurance = !string.IsNullOrEmpty(deliveryAssurance.IONumber) ?
                await _deliveryNoteRepository.GetDeliveryAssuranceAsync(deliveryAssurance.IONumber, deliveryAssurance.LineNumber) :
                new Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance();

            if (action.Equals(DeliveryAssuranceAction.Approve))
            {
                saveDeliveryAssurance.Status = 0;
            }

            saveDeliveryAssurance.SupplierNumber = deliveryAssurance.CustomerNumber;
            saveDeliveryAssurance.IONumber = deliveryAssurance.IONumber;
            saveDeliveryAssurance.Leveransvillkor = deliveryAssurance.TermAndCondition;
            saveDeliveryAssurance.Gardshamtning = deliveryAssurance.Gardshamtning == "1";
            saveDeliveryAssurance.Quantity = deliveryAssurance.Quantity.ConvertToKg();

            saveDeliveryAssurance.Levsatt = deliveryAssurance.LorryType;
            saveDeliveryAssurance.DeliveryAddress = deliveryAssurance.DeliveryAddress;
            saveDeliveryAssurance.ItemName = deliveryAssurance.ItemName;
            saveDeliveryAssurance.Item = deliveryAssurance.Item;
            saveDeliveryAssurance.Sort = deliveryAssurance.Sort;

            saveDeliveryAssurance.SLAM = deliveryAssurance.Slam;
            saveDeliveryAssurance.Straforkortat = deliveryAssurance.Straforkortat;
            saveDeliveryAssurance.Torkat = deliveryAssurance.Torkat;
            saveDeliveryAssurance.RED = deliveryAssurance.Red;

            saveDeliveryAssurance.Ovrigt = deliveryAssurance.OtherInfo;
            saveDeliveryAssurance.Skordear = deliveryAssurance.HarvestYear;
            saveDeliveryAssurance.Warehouse = !string.IsNullOrWhiteSpace(deliveryAssurance.Warehouse) ? deliveryAssurance.Warehouse : DefaultWarehouse;
            saveDeliveryAssurance.Depaavtal = deliveryAssurance.DepaAvtal;
            saveDeliveryAssurance.Leveransdatum = Convert.ToDateTime(deliveryAssurance.DeliveryDate);

            saveDeliveryAssurance.NumbersToUpdate = new string[] { };

            var result = false;
            Session.Remove(sessionKey);
            if (action.Equals(DeliveryAssuranceAction.Change) || action.Equals(DeliveryAssuranceAction.Approve))
            {
                if (action.Equals(DeliveryAssuranceAction.Approve) && Session[nameof(MultiApprove)] != null)
                {
                    var assuranceListDtos = (List<DeliveryAssuranceList>)Session[nameof(MultiApprove)];
                    var assurances = assuranceListDtos.Select(x => $"{x.IONumber}|{x.LineNumber}").ToList();

                    saveDeliveryAssurance.NumbersToUpdate = assurances.ToArray();

                    Session.Remove(nameof(MultiApprove));
                }
                result = await _deliveryNoteRepository.UpdateAsync(saveDeliveryAssurance);
            }
            else
            {
                int noOfItems;
                int.TryParse(noOfNew, out noOfItems);
                var listOfItems = new List<string>();

                for (var i = 0; i < noOfItems; i++)
                {
                    listOfItems.Add("1");
                }

                if (createNew == "1")
                {
                    listOfItems.Add("1"); // Add one more extra to make up for the extras!
                }

                saveDeliveryAssurance.NumbersToUpdate = listOfItems.ToArray();
                var savedDelivery = await _deliveryNoteRepository.SaveNewAsync(saveDeliveryAssurance);
                if (savedDelivery != null)
                {
                    result = true;
                }
            }

            if (result)
            {
                if (createPdf == "1")
                {
                    return
                        Redirect(
                            $"/api/pdfhandler/generatepdf?a={deliveryAssurance.IONumber}&l={deliveryAssurance.LineNumber}&resync=true");
                }

                ViewBag.Message = action.Equals(DeliveryAssuranceAction.Create) ? currentPage.ThankYouText : currentPage.ThankYouTextForUpdate;
                var whSubType = await _deliveryNoteRepository.GetWarehouseSubTypeAsync(saveDeliveryAssurance.Warehouse);
                if (whSubType.Equals("4"))
                {
                    ViewBag.WHSubType = whSubType;
                }
                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceConfirmation.cshtml", new DeliveryAssuranceOverviewViewModel(currentPage));
            }

            ViewBag.Message = currentPage.ErrorText;
            return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssuranceConfirmation.cshtml", new DeliveryAssuranceOverviewViewModel(currentPage)); ;
        }

        [HttpPost]
        [Route("api/delivery-assurance/delete")]
        public async Task<JsonResult> Delete(string ioNumber, int lineNumber)
        {
            if (SiteUser == null)
            {
                return Json(new { success = false, message = "Det gick inte att ta bort raden!"}, JsonRequestBehavior.AllowGet);
            }

            var deleted = await _deliveryNoteRepository.DeleteDeliveryAssuranceAsync(ioNumber, lineNumber);

            return Json(new { success = deleted, message = !deleted ? "Det gick inte att ta bort raden!" : string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/delivery-assurance/get-warehouse")]
        public async Task<JsonResult> GetWarehouseWhenDateChanged(string action, string customerNumber, string itemName,string deliveryDateStr)
        {
            DateTime deliveryDate;

            if (!string.IsNullOrEmpty(itemName) && DateTime.TryParse(deliveryDateStr, out deliveryDate))
            {
                var deliveryAssurance = new Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance()
                {
                    Item = DeliveryAssuranceHelper.ParseItemValue(itemName),
                    Sort = DeliveryAssuranceHelper.ParseSortValue(itemName),
                    Leveransdatum = deliveryDate,
                    SupplierNumber = customerNumber
                };

                var wareHouseList = await GetWarehouseList(deliveryAssurance);

                if (!action.Equals(DeliveryAssuranceAction.Approve))
                {
                    var depaAvtals =  await GetDepaAvtalDelAssAsync(deliveryAssurance, action);

                    return Json(new { warehouses = wareHouseList, depaAvtals = depaAvtals}, JsonRequestBehavior.AllowGet);
                }

                return Json(new {warehouses = wareHouseList}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { warehouses = new List<KeyValuePair<string,string>>() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<ActionResult> GeneratePdf(DeliveryAssuranceListPage currentPage, string a, string l)
        {
            var ioNumber = a;
            int lineNumber;

            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber))
            {
                var deliveryAssurance = await _deliveryNoteRepository.GetDeliveryAssuranceAsync(ioNumber, lineNumber);

                if (deliveryAssurance == null)
                {
                    return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssurancePdf.cshtml", new DeliveryAssuranceOverviewViewModel(currentPage));
                }

                var model = await PopulateOverviewModel(currentPage, deliveryAssurance, true);

                return View("~/Views/AppPages/DeliveryAssurancePage/DeliveryAssurancePdf.cshtml", model);
            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        private async Task<DeliveryAssuranceViewModel> CreateDeliveryAssuranceViewModel(DeliveryAssuranceListPage currentPage,
            Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance deliveryAssurance, string action)
        {
            var depaAvtals = await GetDepaAvtalDelAssAsync(deliveryAssurance, action);

            var sessionKey = $"{nameof(DeliveryAssuranceDetail)}-{action}-{deliveryAssurance.IONumber}";
            var deliveryAssuranceDetail = Session[sessionKey] as DeliveryAssuranceDetail;
            var savedDeliveryAssurance = deliveryAssuranceDetail != null
                ? new Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance()
                {
                    SupplierNumber = deliveryAssuranceDetail.CustomerNumber,
                    IONumber = deliveryAssuranceDetail.IONumber,
                    Item = deliveryAssuranceDetail.Item,
                    Sort = deliveryAssuranceDetail.Sort,
                    Leveransdatum = Convert.ToDateTime(deliveryAssuranceDetail.DeliveryDate),
                    RequestDate = Convert.ToDateTime(deliveryAssuranceDetail.DeliveryDate),
                    OrderTyp = deliveryAssuranceDetail.OrderType,
                    Levsatt = deliveryAssuranceDetail.LorryType,
                    Gardshamtning = deliveryAssuranceDetail.Gardshamtning == "1",
                    Status = deliveryAssuranceDetail.Status
                }: deliveryAssurance;

            var model = new DeliveryAssuranceViewModel(currentPage)
            {
                DeliveryAssurance = deliveryAssuranceDetail ?? new DeliveryAssuranceDetail()
                {
                    CustomerName = GetCustomerName(deliveryAssurance.SupplierNumber),
                    CustomerNumber = deliveryAssurance.SupplierNumber,
                    IONumber = deliveryAssurance.IONumber,
                    LineNumber = deliveryAssurance.LineNumber,

                    TermAndCondition = deliveryAssurance.Leveransvillkor,
                    Gardshamtning = deliveryAssurance.Gardshamtning ? "1" : "0",

                    LorryType = deliveryAssurance.Levsatt,
                    DeliveryAddress = deliveryAssurance.DeliveryAddress,

                    ItemName = deliveryAssurance.ItemName,
                    Item = deliveryAssurance.Item,
                    Sort = deliveryAssurance.Sort,
                    Quantity = deliveryAssurance.Quantity.ConvertToTon(),

                    Slam = deliveryAssurance.SLAM,
                    Straforkortat = deliveryAssurance.Straforkortat,
                    Torkat = deliveryAssurance.Torkat,
                    Red = deliveryAssurance.RED,

                    OtherInfo = !string.IsNullOrWhiteSpace(deliveryAssurance.Ovrigt) ? deliveryAssurance.Ovrigt.ReplaceBrToReturn() : string.Empty,
                    HarvestYear = deliveryAssurance.Skordear,
                    Warehouse = deliveryAssurance.Warehouse != null && deliveryAssurance.Warehouse.Equals(DefaultWarehouse) ? string.Empty : deliveryAssurance.Warehouse,
                    DepaAvtal = deliveryAssurance.Depaavtal,

                    OrderType = deliveryAssurance.OrderTyp,
                    DeliveryType = depaAvtals.Count > 1 ? nameof(DeliveryTypes.Depa) : nameof(DeliveryTypes.Spon),
                    RequestDate = $"{deliveryAssurance.RequestDate:yyyy-MM-dd}",
                    DeliveryDate = action.Equals(DeliveryAssuranceAction.Approve) ? 
                                                    $"{deliveryAssurance.RequestDate:yyyy-MM-dd}" : $"{deliveryAssurance.Leveransdatum:yyyy-MM-dd}",

                    Article = !string.IsNullOrWhiteSpace(deliveryAssurance.Item) && !string.IsNullOrWhiteSpace(deliveryAssurance.Sort) ? $"{deliveryAssurance.Item};-;{deliveryAssurance.Sort}" : string.Empty,
                    Action = action,
                    CurrentUrl = Request?.Url?.PathAndQuery ?? string.Empty,
                    Status = deliveryAssurance.Status
                },
                IsInternal = SettingPage.IsInternal,
                IsNew = action.Equals(DeliveryAssuranceAction.Create),

                LorryTypes = GetLorryTypes(),
                DeliveryAddresses = GetDeliveryAddresses(savedDeliveryAssurance.SupplierNumber),
                WarehouseList = !string.IsNullOrWhiteSpace(savedDeliveryAssurance.Item) && !string.IsNullOrWhiteSpace(savedDeliveryAssurance.Sort) ?
                                await GetWarehouseList(savedDeliveryAssurance) : 
                                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("", "-- Välj mottagningsplats--") },
                //WarehouseList = await GetWarehouseList(savedDeliveryAssurance),
                DepaAvtals = depaAvtals,
                DeliveryValues = GetDeliveryValues(),
                HarvestYears = await GetYears(),
                M3RedValues = await _deliveryNoteRepository.GetM3ValuesAsync(DeliveryAssuranceRadioButtons.Red),
                M3SlamValues = await _deliveryNoteRepository.GetM3ValuesAsync(DeliveryAssuranceRadioButtons.Slam),
                M3StraforkValues = await _deliveryNoteRepository.GetM3ValuesAsync(DeliveryAssuranceRadioButtons.Straforkortat),
                M3TorkatValues = await _deliveryNoteRepository.GetM3ValuesAsync(DeliveryAssuranceRadioButtons.Torkat),
                Articles = GetMergedItems(savedDeliveryAssurance.SupplierNumber, DateTime.Now)
            };

            if (!string.IsNullOrWhiteSpace(model.DeliveryAssurance.IONumber)) //for create from existed delivery, update, approve
            {
                var enalbedDeliveryDate = (SettingPage.IsInternal || model.DeliveryAssurance.OrderType != "206" || !DeliveryAssuranceHelper.IsInStoragePeriod());

                model.EnabledLorryType = model.DeliveryAssurance.OrderType != "206" && model.DeliveryAssurance.LorryType != "630";

                model.EnabledWarehouse =deliveryAssuranceDetail != null ? deliveryAssuranceDetail.EnabledWarehouse == "1" : 
                                        ((SettingPage.IsInternal && model.DeliveryAssurance.OrderType != "206") ||
                                         (!SettingPage.IsInternal && !savedDeliveryAssurance.Gardshamtning &&
                                          DeliveryAssuranceHelper.IsInHarvestPeriod()));
                
                model.EnalbedDeliveryDate = enalbedDeliveryDate;

                model.DeliveryDateCssClass = enalbedDeliveryDate ? GetDatePickerCssClass(action, savedDeliveryAssurance) : string.Empty;
            }
            else //for create new one
            {
                model.EnabledLorryType = model.EnabledWarehouse = model.EnalbedDeliveryDate = true;
                model.DeliveryDateCssClass = GetDatePickerCssClass(action, savedDeliveryAssurance);
            }

            Session.Remove(sessionKey);

            return model;
        }

        private async Task<DeliveryAssuranceOverviewViewModel> PopulateOverviewModelFromDeliveryDetail(DeliveryAssuranceListPage currentPage,
            DeliveryAssuranceDetail deliveryAssurance)
        {
            var lorryType = GetLorryTypes()?.FirstOrDefault(x => x.Key.Equals(deliveryAssurance.LorryType));

            var deliveryAddress = GetDeliveryAddresses(deliveryAssurance.CustomerNumber)?.FirstOrDefault(x => x.Key.Equals(deliveryAssurance.DeliveryAddress));

            if (deliveryAssurance.Action.Equals(DeliveryAssuranceAction.Create))
            {
                deliveryAssurance.Item = DeliveryAssuranceHelper.ParseItemValue(deliveryAssurance.Article);
                deliveryAssurance.Sort = DeliveryAssuranceHelper.ParseSortValue(deliveryAssurance.Article);
                var article = GetMergedItems(deliveryAssurance.CustomerNumber, DateTime.Now)?.FirstOrDefault(d => d.Key == deliveryAssurance.Article);
                deliveryAssurance.ItemName = article?.Value;
            }

            var warehouse =
                (await GetWarehouseList(new Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance
                {
                    Item = deliveryAssurance.Item,
                    Sort = deliveryAssurance.Sort,
                    Leveransdatum = Convert.ToDateTime(deliveryAssurance.DeliveryDate)
                }))?.FirstOrDefault(w => w.Key == GetWarehouseFromDeliveryInformation(deliveryAssurance));

            //var depaAvtals =
            //    (await GetDepaAvtalDelAssAsync(new Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance
            //    {
            //        Item = deliveryAssurance.Item,
            //        Sort = deliveryAssurance.Sort,
            //        Leveransdatum = Convert.ToDateTime(deliveryAssurance.DeliveryDate),
            //        SupplierNumber = deliveryAssurance.CustomerNumber
            //    }));

            var depaAvtals =
                await
                    _deliveryNoteRepository.GetDepaAvtalAsync(deliveryAssurance.CustomerNumber, deliveryAssurance.Item,
                        deliveryAssurance.Sort);

            var depaAvtal = depaAvtals?.FirstOrDefault(d => d.Keyvalue == deliveryAssurance.DepaAvtal);

            var deliveryAssuranceLists = Session[nameof(MultiApprove)] as List<DeliveryAssuranceList>;
            var overviewModel = new DeliveryAssuranceOverviewViewModel(currentPage)
            {
                DeliveryAssurance = new DeliveryAssuranceOverview()
                {
                    CustomerName = deliveryAssurance.CustomerName,
                    CustomerNumber = deliveryAssurance.CustomerNumber,
                    IONumber = deliveryAssurance.IONumber,

                    TermAndCondition = deliveryAssurance.TermAndCondition,
                    Gardshamtning = deliveryAssurance.Gardshamtning == "1",
                    TransportType = GetTransportType(deliveryAssurance.Gardshamtning == "1", deliveryAssurance.TermAndCondition),

                    LorryTypeDesc = lorryType?.Value,
                    DeliveryAddress = deliveryAddress?.Value,

                    ItemName = deliveryAssurance.ItemName,
                    Quantity = deliveryAssurance.Quantity,

                    SlamDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Slam, deliveryAssurance.Slam),
                    StraforkortatDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Straforkortat, deliveryAssurance.Straforkortat),
                    TorkatDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Torkat, deliveryAssurance.Torkat),
                    RedDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Red, deliveryAssurance.Red),

                    OtherInfo = deliveryAssurance.OtherInfo.ReplaceReturnToBr(),
                    HarvestYear = deliveryAssurance.HarvestYear,
                    WarehouseDesc = warehouse?.Value,
                    DepaAvtal = depaAvtal?.Description,

                    DeliveryDate = deliveryAssurance.DeliveryDate,
                    DeliveryTypeDesc = depaAvtals?.Length > 1 ? DeliveryTypes.Depa : DeliveryTypes.Spon,

                    Action = deliveryAssurance.Action
                },
                ChangeUrl = deliveryAssurance.CurrentUrl,
                IsMultiApprove = deliveryAssurance.Action.Equals(DeliveryAssuranceAction.Approve) && deliveryAssuranceLists != null && deliveryAssuranceLists.Count > 1
            };

            var sessionKey = $"{nameof(DeliveryAssuranceDetail)}-{deliveryAssurance.Action}-{deliveryAssurance.IONumber}";
            Session[sessionKey] = deliveryAssurance;

            return overviewModel;
        }

        private async Task<DeliveryAssuranceOverviewViewModel> PopulateOverviewModel(
            DeliveryAssuranceListPage currentPage,
            Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance deliveryAssurance, bool isGeneratePdf = false)
        {
            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            var deliveryAssuranceUrl = urlHelper.ContentUrl(currentPage.ContentLink);

            var lorryType =
                    _deliveryNoteRepository.GetLorryTypes()
                        .FirstOrDefault(x => x.Keyvalue.Equals(deliveryAssurance.Levsatt));

            var deliveryAddress =
                _deliveryNoteRepository.GetDeliveryAdresses(deliveryAssurance.SupplierNumber)
                    .FirstOrDefault(x => x.AddressNumber.Equals(deliveryAssurance.DeliveryAddress));

            var mergedItem =
                _deliveryNoteRepository.GetMergedItems(deliveryAssurance.SupplierNumber, isGeneratePdf ? DateTime.Now : deliveryAssurance.Leveransdatum, !SettingPage.IsInternal)?
                    .FirstOrDefault(i => DeliveryAssuranceHelper.GetItemSort(deliveryAssurance.Item, deliveryAssurance.Sort) == i.Keyvalue);

            var warehouse =
                (await _deliveryNoteRepository.GetWarehouseListAsync(deliveryAssurance.Item, deliveryAssurance.Sort, deliveryAssurance.Leveransdatum))?
                    .FirstOrDefault(w => w.Keyvalue == deliveryAssurance.Warehouse);

            var depaAvtals = await _deliveryNoteRepository.GetDepaAvtalAsync(deliveryAssurance.SupplierNumber, deliveryAssurance.Item, deliveryAssurance.Sort);

            var depaAvtal = depaAvtals.FirstOrDefault(d => d.Keyvalue == deliveryAssurance.Depaavtal);

            var model = new DeliveryAssuranceOverviewViewModel(currentPage)
            {
                DeliveryAssurance = new DeliveryAssuranceOverview()
                {
                    CustomerName = GetCustomerName(deliveryAssurance.SupplierNumber),
                    CustomerNumber = deliveryAssurance.SupplierNumber,
                    IONumber = deliveryAssurance.IONumber,

                    KundorderNr = deliveryAssurance.KundorderNr,
                    GHGvarde = deliveryAssurance.GHGvarde,
                    KundsOrderNr = deliveryAssurance.KundsOrderNr,

                    TermAndCondition = deliveryAssurance.Leveransvillkor,
                    Gardshamtning = deliveryAssurance.Gardshamtning,
                    TransportType = GetTransportType(deliveryAssurance.Gardshamtning, deliveryAssurance.Leveransvillkor),

                    LorryTypeDesc = lorryType?.Description,
                    Address = deliveryAddress ?? new DeliveryAddress(),
                    DeliveryAddress = deliveryAddress != null ? $"{deliveryAddress.Street}, {deliveryAddress.City}" : string.Empty,

                    ItemName = mergedItem?.Description ?? deliveryAssurance.ItemName,
                    Quantity = deliveryAssurance.Quantity.ConvertToTon(),

                    SlamDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Slam, deliveryAssurance.SLAM),
                    StraforkortatDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Straforkortat, deliveryAssurance.Straforkortat),
                    TorkatDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Torkat, deliveryAssurance.Torkat),
                    RedDesc = await _deliveryNoteRepository.GetM3DescriptionAsync(DeliveryAssuranceRadioButtons.Red, deliveryAssurance.RED),

                    OtherInfo = deliveryAssurance.Ovrigt,
                    HarvestYear = deliveryAssurance.Skordear,
                    WarehouseDesc = warehouse?.Description,
                    DepaAvtal = !string.IsNullOrWhiteSpace(depaAvtal?.Keyvalue) ? depaAvtal.Description : string.Empty,

                    DeliveryDate = DeliveryAssuranceHelper.GetValidDate(deliveryAssurance.Leveransdatum),
                    DeliveryTypeDesc = depaAvtals.Length > 1 ? DeliveryTypes.Depa : DeliveryTypes.Spon,
                    Status = deliveryAssurance.Status
                },
                ChangeUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{deliveryAssuranceUrl}Change", new Dictionary<string, string>()
                            {
                                {"a", deliveryAssurance.IONumber},
                                {"l", deliveryAssurance.LineNumber.ToString()}
                            })
            };

            return model;
        }

        #region Helper methods
        private List<KeyValuePair<string, string>> GetLorryTypes()
        {
            var lorryTypes = _deliveryNoteRepository.GetLorryTypes();

            if (SettingPage.IsInternal || string.IsNullOrWhiteSpace(SettingPage.LorryTypesInExternalSite))
                return lorryTypes.Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description)).ToList();

            var arrLorryTypes = SettingPage.LorryTypesInExternalSite.Split(new[] { ',' });
            var selectedLorryTypes = from key in lorryTypes
                                     where arrLorryTypes.Contains(key.Keyvalue)
                                     select key;

            return selectedLorryTypes.Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description)).ToList();
        }

        private List<KeyValuePair<string, string>> GetDeliveryAddresses(string supplier)
        {
            var deliveryAddresses = _deliveryNoteRepository.GetDeliveryAdresses(supplier)?
                                        .Select(i => new KeyValuePair<string, string>(i.AddressNumber, $"{i.Street}, {i.City}")).ToList();
            return deliveryAddresses ?? new List<KeyValuePair<string, string>>();
        }

        private async Task<List<KeyValuePair<string, string>>> GetWarehouseList(
            Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance deliveryAssurance)
        {
            var warehouses = await _deliveryNoteRepository.GetWarehouseListAsync(deliveryAssurance.Item, deliveryAssurance.Sort,
                deliveryAssurance.Leveransdatum);
            if (SettingPage.IsInternal)
                return warehouses.Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description)).ToList();

            var filteredWarehouses = from singleWarehouse in warehouses
                                     let warehouseName = singleWarehouse.Description
                                     where !warehouseName.Contains("z(")
                                     select singleWarehouse;

            return filteredWarehouses.Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description)).ToList();
        }

        private string GetWarehouseFromDeliveryInformation(DeliveryAssuranceDetail assurance)
        {
            if (assurance.Gardshamtning == "1" && SettingPage.IsInternal && string.IsNullOrWhiteSpace(assurance.Warehouse))
            {
                // 2015-08-17 Olle Welwert: Ej hämta mottagningsplatser om ordertyp <> 204.
                if (assurance.OrderType == "204" || !assurance.Action.Equals(DeliveryAssuranceAction.Approve))
                {
                    var iSlam = 2;

                    switch (assurance.Slam)
                    {
                        case "JA-S1":
                            iSlam = 1;
                            break;
                        case "NEJ":
                            iSlam = 0;
                            break;
                    }

                    assurance.Warehouse = _deliveryNoteRepository.GetWarehouse(
                                                assurance.CustomerNumber,
                                                assurance.DeliveryAddress,
                                                assurance.Item,
                                                assurance.Sort,
                                                assurance.Torkat != "NEJ",
                                                iSlam,
                                                assurance.Straforkortat != "NEJ");
                }
            }
            return assurance.Warehouse;
        }

        private async Task<List<KeyValuePair<string, string>>> GetDepaAvtalDelAssAsync(Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance deliveryAssurance, string action)
        {
            var deliveryDate = deliveryAssurance.Leveransdatum;
            if (action.Equals(DeliveryAssuranceAction.Approve))
            {
                deliveryDate = deliveryAssurance.RequestDate;
            }

            var depaAvtals =
                await
                    _deliveryNoteRepository.GetDepaAvtalDelAssAsync(deliveryAssurance.SupplierNumber, deliveryAssurance.Item,
                        deliveryAssurance.Sort, deliveryDate);

            return depaAvtals?.Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description)).ToList();
        }

        private List<KeyValuePair<string, string>> GetDeliveryValues()
        {
            var leverans = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>(nameof(DeliveryTypes.Depa), DeliveryTypes.Depa),
                new KeyValuePair<string,string>(nameof(DeliveryTypes.Spon), DeliveryTypes.Spon)
            };

            return leverans;
        }

        private async Task<List<KeyValuePair<string, string>>> GetYears()
        {
            var result = await _deliveryNoteRepository.GetYearsAsync();
            return result?.Select(i => new KeyValuePair<string, string>(i, i)).ToList() ?? new List<KeyValuePair<string, string>>();
        }

        private List<KeyValuePair<string, string>> GetMergedItems(string supplier, DateTime date) => _deliveryNoteRepository.GetMergedItems(supplier, date, false)?
            .Select(i => new KeyValuePair<string, string>(i.Keyvalue, i.Description))
            .ToList();

        private string GetCustomerName(string customerNo)
        {
            var customer = _deliveryNoteRepository.GetSupplier(customerNo);

            return customer?.SupplierName;
        }

        private string GetTransportType(bool gardshamtning, string leveransvillkor)
        {
            if (gardshamtning)
            {
                return LocalizationService.Current.GetString("/lantBruk/leveransForsakran/create/farmCollection");
            }

            if (string.IsNullOrEmpty(leveransvillkor)) return string.Empty;

            if (leveransvillkor.Equals(DeliveryAssuranceTermConditions.OwnTransport))
            {
                return LocalizationService.Current.GetString("/lantBruk/leveransForsakran/create/ownTransport");
            }

            return leveransvillkor.Equals(DeliveryAssuranceTermConditions.SupplierChoice) ? LocalizationService.Current.GetString("/lantBruk/leveransForsakran/create/supplierChoice") : string.Empty;
        }

        private static string GetDatePickerCssClass(string action, Core.DataModels.DeliveryAssuranceDtos.DeliveryAssurance deliveryAssurance)
        {
            if (action.Equals(DeliveryAssuranceAction.Approve))
            {
                if (SettingPage.IsInternal || (DeliveryAssuranceHelper.IsInStoragePeriod() && deliveryAssurance.OrderTyp == "206"))
                    return string.Empty;

                if (!DeliveryAssuranceHelper.IsInHarvestPeriod())
                    return DeliveryDateCssClass.DateRange;

                if (deliveryAssurance.OrderTyp == "206")
                    return DeliveryDateCssClass.DateHarvest206;

                if (deliveryAssurance.Status < 45)
                    return DeliveryDateCssClass.DateHarvestRange;

                return DeliveryDateCssClass.DateRange;
            }
            else if (action.Equals(DeliveryAssuranceAction.Change) && deliveryAssurance != null)
            {
                if (SettingPage.IsInternal)
                    return DeliveryDateCssClass.DateHigherThanToday;

                if (DeliveryAssuranceHelper.IsInStoragePeriod() && deliveryAssurance.OrderTyp == "206")
                    return string.Empty;

                if (!DeliveryAssuranceHelper.IsInHarvestPeriod())
                    return $"{DeliveryDateCssClass.DateRange} {DeliveryDateCssClass.DateHigherThanToday}";

                if (deliveryAssurance.OrderTyp == "206") //Utsäde (_earlierAssurance.OrderTyp != 204) 204 = spannmål
                    return DeliveryDateCssClass.DateHarvest206;

                if (deliveryAssurance.Status < 45) //När det inte är transportplanerat
                    return DeliveryDateCssClass.DateHarvestRange;

                return $"{DeliveryDateCssClass.DateRange} {DeliveryDateCssClass.DateHigherThanToday}";
            }
            else if (action.Equals(DeliveryAssuranceAction.Create) && deliveryAssurance != null && string.IsNullOrWhiteSpace(deliveryAssurance.IONumber))
            {
                if (SettingPage.IsInternal)
                    return DeliveryDateCssClass.DateHigherThanToday;

                return $"{DeliveryDateCssClass.DateHarvestRange} {DeliveryDateCssClass.DateHigherThanToday}";
            }

            return string.Empty;
        }
        #endregion
    }
}
