using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.Boka;
using Gro.Core.DataModels.Boka.DeleteReservation;
using Gro.Core.DataModels.Boka.ResourceGroupTimes;
using Gro.Core.DataModels.Boka.UpdateReservation;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.Boka;
using Gro.Infrastructure.Data.BokaService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Gro.Core.DataModels.Boka.CustomerSearch;
using Gro.Infrastructure.Data.Centralen_Customer;
using Gro.Infrastructure.Data.Centralen_Items;
using Gro.Core.DataModels.Boka.ListingBoka;
using Gro.Infrastructure.Data.SecurityService;
using Gro.Core.DataModels.Boka.LoadResourceGroups;

namespace Gro.Infrastructure.Data.Repositories
{
    public class BokaRepository : IBokaRepository
    {
        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();
        private readonly BokaSoap _bokaSoap;
        private readonly BokaServiceSoap _bokaServiceSoap;
        private readonly CustomerSoap _customerSoap;
        private readonly ItemsSoap _itemSoap;
        private readonly ISecurityService _securityService;
        private readonly TicketProvider _ticketProvider;
        private string Ticket => _ticketProvider.GetTicket();

        private const string ValueSplitterChar = "-";
        private const string SearchTypeIoNo = "5";
        private const string SearchTypeKoNo = "6";
        private const string SearchTypeDoNo = "7";
        private const string SearchTypeTransmitNo = "8";
        private const string SearchTypeCustomerNo = "9";
        private const int UnKnownErrorStatus = 500;
        private const int OkStatus = 200;
        private const int AccessErrorStatus = 400;
        private const string AgroentanolDivisonsCentralen = "'AE', '050'";
        private const string NormalDivionsCentralen = "'050'";
        public BokaRepository(BokaSoap bokaSoap, BokaServiceSoap bokaServiceSoap, 
            CustomerSoap customerSoap, ItemsSoap itemSoap, ISecurityService securityService,
            TicketProvider ticketProvider)
        {
            _bokaSoap = bokaSoap;
            _bokaServiceSoap = bokaServiceSoap;
            _customerSoap = customerSoap;
            _itemSoap = itemSoap;
            _securityService = securityService;
            _ticketProvider = ticketProvider;
        }

        public List<ResourceItemDto> GetResourceGroupList(bool allResource)
        {
            var isInternal = SettingPage.IsInternal;

            var result = _bokaSoap.GetResourceGroupList(new GetResourceGroupListRequest());
            var resourceGroupList = new List<ResourceItemDto>();
            if (string.IsNullOrWhiteSpace(result?.Body.GetResourceGroupListResult)) return resourceGroupList;
            var xmlStr = result.Body.GetResourceGroupListResult;
            if (!string.IsNullOrEmpty(xmlStr))
            {
                var serializer = new XmlSerializer(typeof(ResourceGroupList));
                ResourceGroupList resultGroupList;

                using (TextReader reader = new StringReader(xmlStr))
                {
                    resultGroupList = (ResourceGroupList)serializer.Deserialize(reader);
                }

                if (resultGroupList?.ResourceGroups?.Count > 0)
                {
                    foreach (var resourceRow in resultGroupList.ResourceGroups)
                    {
                        if (allResource || (!allResource && resourceRow.AllowManualReservations && UserCanMakeBookingsOnResourceGroup(isInternal, resourceRow)))
                        {
                            if (UserCanMakeBookingsOnResourceGroup(isInternal, resourceRow))
                            {
                                var itemResourceType = new ResourceGroupTypeDto
                                {
                                    Id = resourceRow.ResourceGroupType.Id,
                                    Name = resourceRow.ResourceGroupType.Name
                                };

                                var aNode = new ResourceItemDto
                                {
                                    Value = resourceRow.Id,
                                    RegNoMandatory = resourceRow.RegNoMandatory,
                                    Url = !resourceRow.WebPage.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) ? string.Format("http://{0}", resourceRow.WebPage) : resourceRow.WebPage,
                                    M3Id = resourceRow.M3Id,
                                    Name = resourceRow.Name,
                                    RowType = itemResourceType
                                };
                                resourceGroupList.Add(aNode);
                            }
                        }
                    }
                }
            }
            return resourceGroupList;
        }

        public List<ResourceItemDto> GetResourceGroupsOnIo(string warehouseId)
        {
            var isInternal = SettingPage.IsInternal;

            var result = _bokaSoap.GetResourceGroupList(new GetResourceGroupListRequest());
            var resourceGroupList = new List<ResourceItemDto>();
            if (string.IsNullOrWhiteSpace(result?.Body.GetResourceGroupListResult)) return resourceGroupList;
            var xmlStr = result.Body.GetResourceGroupListResult;

            if(!string.IsNullOrEmpty(xmlStr))
            {
                var serializer = new XmlSerializer(typeof(ResourceGroupList));
                ResourceGroupList resultGroupList;

                using (TextReader reader = new StringReader(xmlStr))
                {
                    resultGroupList = (ResourceGroupList)serializer.Deserialize(reader);
                }

                if(resultGroupList?.ResourceGroups?.Count > 0)
                {
                    foreach(var resourceRow in resultGroupList.ResourceGroups)
                    {
                        if(resourceRow.StorageAreaNo == warehouseId || (!string.IsNullOrEmpty(resourceRow.Id) && resourceRow.Id == warehouseId))
                        {
                            if(UserCanMakeBookingsOnResourceGroup(isInternal, resourceRow))
                            {
                                var itemResourceType = new ResourceGroupTypeDto
                                {
                                    Id = resourceRow.ResourceGroupType.Id,
                                    Name = resourceRow.ResourceGroupType.Name
                                };

                                var aNode = new ResourceItemDto
                                {
                                    Value = resourceRow.Id,
                                    RegNoMandatory = resourceRow.RegNoMandatory,
                                    Url  = !resourceRow.WebPage.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) ? string.Format("http://{0}", resourceRow.WebPage) : resourceRow.WebPage,
                                    M3Id = resourceRow.M3Id,
                                    Name = resourceRow.Name,
                                    RowType = itemResourceType
                                };
                                resourceGroupList.Add(aNode);
                            }
                        }
                    }
                }
            }
            if(resourceGroupList.Count == 0)
            {
                return GetResourceGroupList(false);
            }
            return resourceGroupList;
        }
        private bool UserCanMakeBookingsOnResourceGroup(bool isInternal, ResourceGroup resourceRow)
        {
            if(isInternal)
            {
                if(resourceRow.Visibility.Inside && !resourceRow.Visibility.InsideDeliveryPlanGroup)
                {
                    return true;
                }
            }
            else
            {
                if(resourceRow.Visibility.Outside)
                {
                    return true;
                }
            }
            return false;
        }

        public ResourceGroupItemsDto LoadItemsOnresourceGroup(string resourceGroupId, string selectedDate, string currentArticleItem, bool showOnlyUnloadingItems)
        {
            DateTime fromdate;
            var resourceGroupItems = new ResourceGroupItemsDto();
            if (DateTime.TryParse(selectedDate, out fromdate))
            {
                var todate = fromdate.AddDays(1);
                var resultItems = this.GetResursgruppItem(resourceGroupId, fromdate, todate);
                var resultVehicles = this.GetVehicles(resourceGroupId);

                XmlDocument xmlDoc = new XmlDocument();
                //get list item
                xmlDoc.LoadXml(resultItems);

                if (xmlDoc.DocumentElement != null)
                {
                    var nodeItemList = xmlDoc.SelectNodes("/ResourceGroupItems/Item");
                    var items = new List<DropDownDto>();
                    if (nodeItemList != null)
                        foreach (XmlNode node in nodeItemList)
                        {
                            var dried = this.FnDriedToString(node["Dried"]?.InnerText);
                            var itemNo = node["ItemNo"]?.InnerText;
                            var sort = node["Sort"]?.InnerText;
                            var name = node["Name"]?.InnerText;
                            var unitName = node["UnitName"]?.InnerText;
                            var id = node["ID"]?.InnerText;
                            var item = new DropDownDto()
                            {
                                Display = $"{itemNo} - {name} {sort} - {dried}",
                                Value = string.Format("{0}{2}{1}{2}{3}{2}{4}{2}{5}", itemNo, dried, ValueSplitterChar, sort, unitName, id.ToLower()),
                            };
                            item.IsSelected = item.Value.StartsWith(currentArticleItem, StringComparison.InvariantCultureIgnoreCase) &&
                                              !string.IsNullOrEmpty(currentArticleItem) ? true : false;

                            if((showOnlyUnloadingItems && node["Unloading"]?.InnerText.ToLower() == "true") || (!showOnlyUnloadingItems && node["Loading"]?.InnerText.ToLower() == "true"))
                                items.Add(item);
                        }
                    resourceGroupItems.Items = items;

                    //get reservation Stop
                    var resourceGroupHasReservationStop = false;
                    var nodeReservationStopList = xmlDoc.SelectNodes("/ResourceGroupItems/ReservationStops/Resources/ResourceReservationStop");
                    if (nodeReservationStopList != null)
                    {
                        foreach (XmlNode node in nodeReservationStopList)
                        {
                            var aReservationStop = new ReservationStopDto
                            {
                                ResourceName = node["Name"]?.InnerXml,
                                Message = node["Reason"]?.InnerXml,
                                FromDate = node["FromReservationStopDate"]?.ChildNodes[0].InnerText,
                                FromTime = node["FromReservationStopDate"]?.ChildNodes[1].InnerText,
                            };
                            resourceGroupHasReservationStop = node["ToReservationStopDate"] == null;

                            var toDateString = node["ToReservationStopDate"]?.ChildNodes[0].InnerText;
                            var toTimeString = node["ToReservationStopDate"]?.ChildNodes[1].InnerText;

                            DateTime tmpDate;
                            DateTime? dateToCheckIfIsInFeature = null;
                            if (DateTime.TryParse(toDateString, out tmpDate))
                            {
                                if (toTimeString?.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) > 0)
                                {
                                    int hour;
                                    if (int.TryParse(toTimeString.Split(new char[] {':'})[0], out hour))
                                    {
                                        int min;
                                        dateToCheckIfIsInFeature = int.TryParse(toTimeString.Split(new char[] {':'})[1], out min) ? 
                                            new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, min, 0) : 
                                            new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, 23, 59, 0);
                                    }
                                }
                            }
                            if (dateToCheckIfIsInFeature.HasValue)
                            {
                                if (dateToCheckIfIsInFeature >= DateTime.Now)
                                {
                                    aReservationStop.ToDate = node["ToReservationStopDate"]?.ChildNodes[0].InnerText;
                                    aReservationStop.ToTime = node["ToReservationStopDate"]?.ChildNodes[1].InnerText;
                                    resourceGroupHasReservationStop = true;
                                }
                            }
                            if (resourceGroupHasReservationStop)
                            {
                                resourceGroupItems.ReservationStops.Add(aReservationStop);
                            }
                        }
                    }
                    if (!resourceGroupHasReservationStop)
                    {
                        
                    }
                }
                
                //get list vehicles
                xmlDoc.RemoveAll();
                xmlDoc.LoadXml(resultVehicles);
                if(xmlDoc.DocumentElement != null)
                {
                    var nodeItemList = xmlDoc.SelectNodes("/ResourceGroupVehicleTypes/ResourceGroupVehicleType");
                    var vehicles = new List<DropDownDto>();
                    if (nodeItemList != null)
                        vehicles.AddRange(from XmlNode node in nodeItemList
                            select new DropDownDto()
                            {
                                Display = node["Name"]?.InnerText ?? string.Empty, Value = node["AssortmentID"]?.InnerText ?? string.Empty
                            });
                    resourceGroupItems.Vehicles = vehicles;
                }
            }
            return resourceGroupItems;
        }

        private string GetResursgruppItem(string resourceGroupId, DateTime fromdate, DateTime toDate)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            XmlNode getResourceGroupItems = xmlDoc.CreateElement("GetResourceGroupItems");
            XmlNode resursgrupp = xmlDoc.CreateElement("ResourceGroup");
            resursgrupp.InnerText = resourceGroupId;
            getResourceGroupItems.AppendChild(resursgrupp);

            //****************** FrånDatum *******************
            //** Huvudnode From
            XmlNode from = xmlDoc.CreateElement("From");

            //** Childnode Datum
            XmlNode date = xmlDoc.CreateElement("Date");
            from.AppendChild(date);

            //** Från år
            XmlNode dateYear = xmlDoc.CreateElement("Year");
            dateYear.InnerXml = fromdate.Year.ToString();
            date.AppendChild(dateYear);

            //** Från Månad
            XmlNode dateMonth = xmlDoc.CreateElement("Month");
            dateMonth.InnerXml = fromdate.Month.ToString();
            date.AppendChild(dateMonth);

            //** Från Dag
            XmlNode dateDay = xmlDoc.CreateElement("Day");
            dateDay.InnerXml = fromdate.Day.ToString();
            date.AppendChild(dateDay);

            //** Lägg till FrånDatum
            getResourceGroupItems.AppendChild(from);

            //************** Tilldatum *********************
            //** Huvudnode TO
            XmlNode to = xmlDoc.CreateElement("To");

            //** Childnode Datum
            XmlNode _to_date = xmlDoc.CreateElement("Date");
            to.AppendChild(_to_date);


            //** To år
            XmlNode toDateYear = xmlDoc.CreateElement("Year");
            toDateYear.InnerXml = toDate.Year.ToString();
            _to_date.AppendChild(toDateYear);

            //** To Månad
            XmlNode toDateMonth = xmlDoc.CreateElement("Month");
            toDateMonth.InnerXml = toDate.Month.ToString();
            _to_date.AppendChild(toDateMonth);

            //** To Dag
            XmlNode toDateDay = xmlDoc.CreateElement("Day");
            toDateDay.InnerXml = toDate.Day.ToString();
            _to_date.AppendChild(toDateDay);

            //** Lägg till To Datum
            getResourceGroupItems.AppendChild(to);

            xmlDoc.AppendChild(getResourceGroupItems);

            var answer = _bokaSoap.GetResourceGroupItems(new GetResourceGroupItemsRequest()
            {
                Body = new GetResourceGroupItemsRequestBody()
                {
                    xml = xmlDoc.InnerXml
                }
            });

            return !string.IsNullOrWhiteSpace(answer?.Body.GetResourceGroupItemsResult) ? answer.Body.GetResourceGroupItemsResult : string.Empty;
        }
        
        private string GetVehicles(string resourceGroupId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            XmlNode getResourceGroupVehicleTypes = xmlDoc.CreateElement("GetResourceGroupVehicleTypes");

            //****************** Resursgruppen *******************
            XmlNode resourceGroup = xmlDoc.CreateElement("ResourceGroup");
            resourceGroup.InnerText = resourceGroupId;
            getResourceGroupVehicleTypes.AppendChild(resourceGroup);

            //*** Lägger till hela Roten till dokumentet
            xmlDoc.AppendChild(getResourceGroupVehicleTypes);

            //** Returnera sträng från WS
            var resourceGroupVehicleTypes = _bokaSoap.GetResourceGroupVehicleTypes(new GetResourceGroupVehicleTypesRequest() {
                Body = new GetResourceGroupVehicleTypesRequestBody()
                {
                    xml = xmlDoc.InnerXml
                }
            });

            return !string.IsNullOrWhiteSpace(resourceGroupVehicleTypes?.Body.GetResourceGroupVehicleTypesResult) ? 
                resourceGroupVehicleTypes.Body.GetResourceGroupVehicleTypesResult : string.Empty;
        }
        private string FnDriedToString(string torkad)
        {
            return string.CompareOrdinal(torkad.ToLower().Trim(), "true") == 0 || string.CompareOrdinal(torkad.ToLower().Trim(), "ja") == 0 ? "Torkad" : "Otorkad";
        }

        public SearchResultDto CustomerSearch(string resourceGroupId, string searchString, string searchType, string loadOrUnlodValue, string customerSearcType, string customerNo, string username)
        {
            var isInternal = SettingPage.IsInternal;
            var currentUser = customerNo;
            if (isInternal)
            {
                currentUser = "Intern";
            }
            var loading = loadOrUnlodValue == "1";

            var result = new SearchResultDto() {
                BookingOrder = new BookingOrder()
            };

            searchString = searchString.Replace("drop", "").Replace(";", "").Replace("=", "").Replace("truncate", "").Replace("delete", "");
            int qtyMultiplicator;
            switch(searchType)
            {
                case SearchTypeIoNo:
                    result.BookingOrder = GetBookingOrderByIoNumber(searchString, int.Parse(searchType));
                    if(result.BookingOrder != null)
                    {
                        if(result.BookingOrder.Supplier.Equals(currentUser) || isInternal)
                        {
                            var url = ConfigurationManager.AppSettings["UrlToDeliveryAssurance"];
                            var lineNo = result.BookingOrder.Linenumber.ToString();
                            result.LinkUrl = url + "?u=t&a=" + searchString + "&l=" + lineNo;

                            if(!string.IsNullOrEmpty(result.BookingOrder.Supplier))
                            {
                                result.Customers = GetCustomers(result.BookingOrder.Supplier, customerSearcType,
                                    isInternal, username);
                            }
                        }
                        else
                        {
                            result.Customers = null;
                        }
                    }
                    break;
                case SearchTypeKoNo:
                    qtyMultiplicator = 1;
                    var resultMageReservation = GetReservationForm(SearchTypeKoNo, searchString, loading);
                    if (resultMageReservation?.Error.Count == 0)
                    {
                        var customerNumber = string.Empty;
                        if (resultMageReservation.Item.Count > 0)
                        {
                            var itemRow = resultMageReservation.Item[0];
                            result.BookingOrder.ItemNumber = itemRow.ItemNo;
                            result.BookingOrder.Item = itemRow.Name;
                            result.BookingOrder.Sort = itemRow.Sort;
                            result.BookingOrder.Linenumber = 1;
                            result.BookingOrder.Torkat = itemRow.Dried == "true";
                            if (itemRow.UnitName.Equals("ton", StringComparison.CurrentCultureIgnoreCase))
                            {
                                qtyMultiplicator = 1000;
                            }
                        }
                        foreach (var reservationFormRow in resultMageReservation.FromData)
                        {
                            if (
                                !reservationFormRow.CustomerNo.Equals(currentUser,
                                    StringComparison.InvariantCultureIgnoreCase) && !isInternal) continue;
                            result.BookingOrder.Quantity = Convert.ToInt32(int.Parse(reservationFormRow.Quantity) * qtyMultiplicator);
                            result.BookingOrder.DeliveryDate = Convert.ToDateTime(reservationFormRow.PlannedDate);
                            result.BookingOrder.Warehouse = reservationFormRow.ResourceGroupId;

                            customerNumber = reservationFormRow.CustomerNo;
                            result.RegNo = reservationFormRow.RegNo;
                        }

                        if (!string.IsNullOrEmpty(customerNumber))
                        {
                            result.Customers = GetCustomers(customerNumber, customerSearcType, isInternal, username);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(resultMageReservation?.Error[0].Id) > 0)
                        {
                            result.ErrorMessage = resultMageReservation != null ? resultMageReservation.Error[0].Message : string.Empty;
                            result.Status = UnKnownErrorStatus;
                        }
                    }
                    break;
                case SearchTypeDoNo:
                    var rs = GetReservationForm(SearchTypeDoNo, searchString, loading);
                    qtyMultiplicator = 1;
                    if (rs.Error.Count == 0)
                    {
                        var customerNumber = string.Empty;
                        if (rs.Item.Count > 0)
                        {
                            var itemRow = rs.Item[0];
                            result.BookingOrder.ItemNumber = itemRow.ItemNo;
                            result.BookingOrder.Item = itemRow.Name;
                            result.BookingOrder.Sort = itemRow.Sort;
                            result.BookingOrder.Linenumber = 1;
                            result.BookingOrder.Torkat = itemRow.Dried == "true";
                            if (itemRow.UnitName.Equals("ton", StringComparison.CurrentCultureIgnoreCase))
                            {
                                qtyMultiplicator = 1000;
                            }
                        }
                        foreach (var reservationFormRow in rs.FromData)
                        {
                            if (!reservationFormRow.CustomerNo.Equals(currentUser,
                                    StringComparison.InvariantCultureIgnoreCase) && !isInternal) continue;
                            result.BookingOrder.Quantity =
                                Convert.ToInt32(int.Parse(reservationFormRow.Quantity) * qtyMultiplicator);
                            result.BookingOrder.DeliveryDate = Convert.ToDateTime(reservationFormRow.PlannedDate);
                            customerNumber = reservationFormRow.CustomerNo;
                            result.RegNo = reservationFormRow.RegNo;
                            result.BookingOrder.Warehouse = reservationFormRow.ResourceGroupId;
                        }
                        if (customerNumber?.Length == 3)
                        {
                            customerNumber = $"WHL00{customerNumber}";
                        }
                        result.Customers = GetCustomers(customerNumber, customerSearcType, isInternal, username);
                    }
                    else
                    {
                        if (Convert.ToInt32(rs.Error[0].Id) > 0)
                        {
                            result.ErrorMessage = rs.Error[0].Message;
                            result.Status = UnKnownErrorStatus;
                        }
                    }
                    break;
                case SearchTypeTransmitNo:
                    var resultSearch = GetReservationForm(SearchTypeTransmitNo, searchString, loading);
                    qtyMultiplicator = 1;
                    if (resultSearch.Error.Count == 0)
                    {
                        var customerNumber = string.Empty;
                        if (resultSearch.Item.Count > 0)
                        {
                            var itemRow = resultSearch.Item[0];
                            result.BookingOrder.ItemNumber = itemRow.ItemNo;
                            result.BookingOrder.Item = itemRow.Name;
                            result.BookingOrder.Sort = itemRow.Sort;
                            result.BookingOrder.Linenumber = 1;
                            result.BookingOrder.Torkat = itemRow.Dried == "true";
                            if (itemRow.UnitName.Equals("ton", StringComparison.CurrentCultureIgnoreCase))
                            {
                                qtyMultiplicator = 1000;
                            }
                        }
                        foreach (var reservationFormRow in resultSearch.FromData)
                        {
                            result.BookingOrder.Quantity =
                                Convert.ToInt32(int.Parse(reservationFormRow.Quantity) * qtyMultiplicator);
                            result.BookingOrder.DeliveryDate = Convert.ToDateTime(reservationFormRow.PlannedDate);
                            result.BookingOrder.Warehouse = reservationFormRow.ResourceGroupId;
                            customerNumber = reservationFormRow.CustomerNo;
                            result.RegNo = reservationFormRow.RegNo;
                        }
                        if (customerNumber?.Length == 3)
                        {
                            customerNumber = $"WHL00{customerNumber}";
                        }
                        result.Customers = GetCustomers(customerNumber, customerSearcType, isInternal, username);
                    }
                    else
                    {
                        switch (Convert.ToInt32(resultSearch.Error[0].Id))
                        {
                            case 0:
                                break;
                            case 4:
                                result.ErrorMessage = "Resurs för denna sändning kunde inte hittas.";
                                result.Status = UnKnownErrorStatus;
                                break;
                            default:
                                result.ErrorMessage = resultSearch.Error[0].Message;
                                result.Status = UnKnownErrorStatus;
                                break;
                        }
                    }
                    break;
                case SearchTypeCustomerNo:
                    if (string.IsNullOrEmpty(currentUser))
                    {
                        result.Status = UnKnownErrorStatus;
                        result.ErrorMessage = "Du är inte inloggad!";
                        return result;
                    }
                    if (!isInternal)
                    {
                        searchString = currentUser;
                    }
                    result.Customers = GetCustomers(searchString, customerSearcType, isInternal, username);

                    if (result.Customers.Count == 0)
                    {
                        result.Customers = GetInternalTransport(searchString);
                    }
                    if (customerSearcType == "1" && result.Customers?.Count >= 1)
                    {
                        var iOs = GetCustomerIOs(result.Customers[0].CustomerNo);
                        result.Ios = iOs;
                    }
                    result.Status = OkStatus;
                    break;
            }
            return result;
        }

        private List<CustomerDto> GetInternalTransport(string customerNoToSearchFor)
        {
            var result = new List<CustomerDto>();
            var doc = new XmlDocument();
            doc.LoadXml("<Items><param type='' name='Warehouse' operator='AND' condition='='>'" + customerNoToSearchFor + "'</param></Items>");
            var request = XElement.Load(new XmlNodeReader(doc));
            var dataDoc = _itemSoap.getLagerstalle(new getLagerstalleRequest()
            {
                Body = new getLagerstalleRequestBody()
                {
                    inParam = request
                }
            });
            if(dataDoc?.Body?.getLagerstalleResult != null)
            {
                var aNode = new CustomerDto
                {
                    CustomerNo = customerNoToSearchFor,
                    Name = dataDoc?.Body?.getLagerstalleResult?.Value ?? string.Empty,
                    Status = OkStatus,
                };
                result.Add(aNode);
            }
            return result;
        }

        private List<IODto> GetCustomerIOs(string customerNo)
        {
            var result = _bokaServiceSoap.getIOlist(new getIOlistRequest()
            {
                Body = new getIOlistRequestBody()
                {
                    Supplier = customerNo
                }
            });
            var ioDtos = new List<IODto>();
            var ioDto = new IODto();
            var deleveryAssuranceLists = result?.Body.getIOlistResult.ToList();
            if (deleveryAssuranceLists == null) return ioDtos;
            foreach (var io in deleveryAssuranceLists)
            {
                try
                {
                    ioDto = new IODto()
                    {
                        IONumber = io.IOnumber,
                        ItemName = io.Itemname,
                        LineNumber = io.LineNumber,
                        Quantity = io.Quantity,
                        Status = io.Status,
                        Warehouse = io.Warehouse,
                        WarehouseName = GetWarehouseName(io.Warehouse)
                    };
                }
                catch (Exception)
                {
                    ioDto.WarehouseName = "Saknas";
                }
                ioDtos.Add(ioDto);
            }
            return ioDtos;
        }

        private string GetWarehouseName(string wareHouseId)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<Items><param type='' name='Warehouse' operator='AND' condition='='>'" + wareHouseId + "'</param></Items>");
            var request = XElement.Load(new XmlNodeReader(doc));
            var dataDoc = _itemSoap.getLagerstalle(new getLagerstalleRequest()
            {
                Body = new getLagerstalleRequestBody()
                {
                    inParam = request
                }
            });
            var items = dataDoc?.Body?.getLagerstalleResult;
            return items?.Element("Item")?.Element("WarehouseDescription")?.Value ?? string.Empty;
        }

        private MakeReservationFormData GetReservationForm(string referenceType, string referenceNumber, bool loading)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            XmlNode getMakeReservationFormData = xmlDoc.CreateElement("GetMakeReservationFormData");
            XmlNode reservationFormNode;
            switch (referenceType)
            {
                case SearchTypeKoNo:
                    reservationFormNode = GetXmlNode(referenceNumber, loading, "CustomerOrder", "OrderNumber", xmlDoc);
                    break;
                case SearchTypeDoNo:
                    reservationFormNode = GetXmlNode(referenceNumber, loading, "DistributionOrder", "OrderNumber", xmlDoc);
                    break;
                case SearchTypeTransmitNo:
                    reservationFormNode = GetXmlNode(referenceNumber, loading, "Shipment", "ShipmentNumber", xmlDoc);
                    break;
                default:
                    reservationFormNode = GetXmlNode(referenceNumber, loading, "CustomerOrder", "OrderNumber", xmlDoc);
                    break;
            }
            getMakeReservationFormData.AppendChild(reservationFormNode);
            xmlDoc.AppendChild(getMakeReservationFormData);

            var reservationFormData = _bokaSoap.GetMakeReservationFormData(new GetMakeReservationFormDataRequest()
            {
                Body = new GetMakeReservationFormDataRequestBody(xmlDoc.InnerXml)
            });

            var serializer = new XmlSerializer(typeof(MakeReservationFormData));
            MakeReservationFormData result = null;
            if (!string.IsNullOrEmpty(reservationFormData?.Body?.GetMakeReservationFormDataResult))
            {
                using (TextReader reader = new StringReader(reservationFormData.Body.GetMakeReservationFormDataResult))
                {
                    result = (MakeReservationFormData)serializer.Deserialize(reader);
                }
            }
            return result;
        }

        private XmlNode GetXmlNode(string referenceNumber, bool loading, string nodeName, string numberType,
            XmlDocument xmlDoc)
        {
            XmlNode reservationFormNode = xmlDoc.CreateElement(nodeName);

            XmlNode customerOrderOrderNumber = xmlDoc.CreateElement(numberType);
            customerOrderOrderNumber.InnerText = referenceNumber;

            XmlNode customerOrderLoadingOrUnloading = xmlDoc.CreateElement("LoadingOrUnloading");
            customerOrderLoadingOrUnloading.InnerText = loading ? "Loading" : "Unloading";

            reservationFormNode.AppendChild(customerOrderOrderNumber);
            reservationFormNode.AppendChild(customerOrderLoadingOrUnloading);

            return reservationFormNode;
        }

        public List<CustomerDto> GetCustomers(string customerNo, string customerSearchType, bool isInternal, string currentUser)
        {
            List<CustomerDto> result = new List<CustomerDto>();
            var strDivisions = customerSearchType.Equals("1") ? NormalDivionsCentralen : AgroentanolDivisonsCentralen;
            var doc = new XmlDocument();
            doc.LoadXml(
                "<Customers><param type='' name='kundnummerID' operator='AND' condition='='>'" + customerNo + "'</param>"
                + "<param type='' name='Division' operator='AND' condition='IN'>(" + strDivisions + ")</param></Customers>"
                );
            var request = new getKund_BokaRequest(
                new getKund_BokaRequestBody(XElement.Parse(doc.OuterXml))
                );
            var dataDoc = _customerSoap.getKund_Boka(request);
            if (dataDoc?.Body?.getKund_BokaResult != null)
            {
                var serializer = new XmlSerializer(typeof(CustomerSearch));
                CustomerSearch resultDataCustomers;

                using (TextReader reader = new StringReader(dataDoc.Body.getKund_BokaResult.ToString()))
                {
                    resultDataCustomers = (CustomerSearch)serializer.Deserialize(reader);
                }
                result = resultDataCustomers.Customers;
            }
            if (result.Count > 0)
            {
                var siteUserInfo = _securityService.GetUser(new RequestUser
                {
                    UserId = currentUser
                }, Ticket);
                result[0].Email = siteUserInfo.Email ?? string.Empty;
                result[0].MobileNo = siteUserInfo.PhoneMobile ?? string.Empty;
            }
            return result;
        }
        private BookingOrder GetBookingOrderByIoNumber(string searchString, int searchType)
        {
            var result = _bokaServiceSoap.getIO(new getIORequest()
            {
                Body = new getIORequestBody()
                {
                    SearchString = searchString,
                    Typ = searchType
                }
            });
            return result != null ? result.Body.getIOResult : new BookingOrder();
        }

        public List<ResourceGroupItemDto> SearchAvailbleSlots(string resourceGroupId, string selectedDate, string article, string qty, string loadunload, string veichleType, string driedUnDried, string customerNo, string searchType)
        {
            var isInternal = SettingPage.IsInternal;
            var result = new List<ResourceGroupItemDto>();

            var artikelId = ExtractArticleId(article);
            var torkad = GetSecondValue(article).Equals("torkad", StringComparison.CurrentCultureIgnoreCase);
            var kvantitet = decimal.Parse(qty.Replace(".", ","));
            var dateFrom = Convert.ToDateTime(selectedDate);
            var dateTo = dateFrom.AddDays(1);
            var unit = ExtractUnit(article);

            if (unit.Equals("kg", StringComparison.InvariantCultureIgnoreCase))
            {
                kvantitet = kvantitet * 1000;
            }
            var xmlString = GetResourceGroupTimes(new RequestGroupTime() {
                ResourceGroup = resourceGroupId,
                ItemId = artikelId,
                CustomerNo = customerNo,
                Dried = torkad,
                Quantity = kvantitet,
                VehicleTypeId = veichleType,
                _Loading = true,
                _Unloading = true,
                DateFrom = dateFrom,
                DateTo = dateTo,
                Loading = loadunload == "1",
                Unloading = loadunload == "2"

            });
            var resourceGroupTimes = GetTimes(xmlString);
            var listGroupTime = loadunload.Equals("1") ? resourceGroupTimes.Loading : resourceGroupTimes.Unloading;

            var itemCount = 0;
            if (listGroupTime == null) return result;
            foreach (var item in listGroupTime)
            {
                ResourceGroupItemDto timeRow;
                if(item.Reservation != null)
                {
                    timeRow = new ResourceGroupItemDto()
                    {
                        IsBooked = item.Reservation != null,
                        FromDateTime = item.Reservation?.StartTime?.Time != null ?
                            DateTime.Parse(item.Reservation.StartTime.Time.Hour + ":" + item.Reservation.StartTime.Time.Minute).ToString("HH:mm") :
                            DateTime.Parse("00:00").ToString("HH:mm"),
                        FromDate = item.Reservation?.StartTime?.Time != null ?
                            DateTime.Parse(item.Reservation.StartTime.Date.Year + "-" + item.Reservation.StartTime.Date.Month + "-" + item.Reservation.StartTime.Date.Day).ToShortDateString() : 
                            string.Empty,
                        ToDateTime = item.Reservation?.EndTime?.Time != null ?
                            DateTime.Parse(item.Reservation.EndTime.Time.Hour + ":" + item.Reservation.EndTime.Time.Minute).ToString("HH:mm") :
                            DateTime.Parse("00:00").ToString("HH:mm"),
                        LicensePlateNo = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, item.Reservation.LicensePlateNo),
                        CustomerNo = HttpUtility.HtmlEncode(item.Reservation.CustomerNo),
                        CustomerName = HttpUtility.HtmlEncode(item.Reservation.CustomerName),
                        Owner = item.Reservation.Owner,
                    };


                    var dateComp = Convert.ToDateTime($"{timeRow.FromDate} {timeRow.FromDateTime}");
                    var minutesToReservation = (dateComp - DateTime.Now).TotalMinutes;
                    if (IsEditingEnabledForReservation(minutesToReservation, isInternal, customerNo, item.Reservation.Owner.Trim(), item.Reservation.CustomerNo))
                    {
                        timeRow.UserCanChange = true;
                    }
                    timeRow.ToolTip = "";

                    foreach (var reservationItem in item.Reservation.ReservationItems)
                    {
                        timeRow.Sort = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.Sort);
                        timeRow.KontraktArtikel = reservationItem.ItemNo + " - " + reservationItem.ItemName;
                        timeRow.Qty = reservationItem.Quantity.ToString(CultureInfo.CurrentCulture);
                        timeRow.Unit = reservationItem.UnitName;
                        timeRow.ItemName = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.ItemName);
                        timeRow.ItemID = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.ItemID);
                        timeRow.ItemNo = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.ItemNo);
                        timeRow.Unit = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.UnitName);
                        timeRow.Qty = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, reservationItem.Quantity.ToString(CultureInfo.CurrentCulture));
                        timeRow.ResourceId = item.Resource;
                        timeRow.ResourceName = item.ResourceName;
                        timeRow.Loading = item.Reservation.Loading;
                        timeRow.Unloading = item.Reservation.Unloading;
                        timeRow.Leveransforsakransnr = HttpUtility.HtmlEncode(item.Reservation.Leveransforsakransnr.Trim());
                        timeRow.MobileNo = HttpUtility.HtmlEncode(item.Reservation.MobileNo.Trim());
                        timeRow.Note = System.Net.WebUtility.HtmlDecode(item.Reservation.Note.Trim());
                        timeRow.ReminderMinutesBefore = item.Reservation.ReminderMinutesBefore;
                        timeRow.EmailAdress = HttpUtility.HtmlEncode(item.Reservation.EmailAddress.Trim());
                        timeRow.ReminderEmail = item.Reservation.ReminderEmail.ToString();
                        timeRow.ReservationId = item.Reservation.ID.Trim();
                        timeRow.CustomerNo = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, item.Reservation.CustomerNo);
                        timeRow.CustomerName = FilterFieldForDisplayByPermissions(isInternal, item, customerNo, item.Reservation.CustomerName);
                        timeRow.SpeditorNo = HttpUtility.HtmlEncode(item.Reservation.SpeditorNo.Trim());
                        timeRow.ContractNo = HttpUtility.HtmlEncode(item.Reservation.ContractNo);

                        itemCount += 1;
                        if (itemCount > 0)
                        {
                            result.Add(timeRow);
                            timeRow = new ResourceGroupItemDto
                            {
                                FromDateTime = string.Empty,
                                ToDateTime = string.Empty,
                                CustomerName = string.Empty
                            };
                        }
                    }
                    itemCount = 0;
                }
                else
                {
                    timeRow = new ResourceGroupItemDto()
                    {
                        FromDateTime = item.DateFrom?.Time != null ?
                            DateTime.Parse(item.DateFrom.Time.Hour + ":" + item.DateFrom.Time.Minute).ToString("HH:mm") : 
                            DateTime.Parse("00:00").ToString("HH:mm"),
                        ToDateTime = item.DateTo?.Time != null ?
                            DateTime.Parse(item.DateTo.Time.Hour + ":" + item.DateTo.Time.Minute).ToString("HH:mm") :
                            DateTime.Parse("00:00").ToString("HH:mm"),
                        ResourceId = item.Resource,
                    };
                    result.Add(timeRow);
                }
            }
            return result;
        }

        private string FilterFieldForDisplayByPermissions(bool isInternal, ResourceGroupTime row, string customerNo, string value)
        {
            if(isInternal)
            {
                return value;
            }
            else
            {
                if(row.Reservation.CustomerNo.Equals(customerNo, StringComparison.InvariantCultureIgnoreCase))
                {
                    return value;
                }
            }
            return string.Empty;
        }
        private string ExtractArticleId(string aString)
        {
            if(aString.IndexOf(ValueSplitterChar, StringComparison.Ordinal) >= 0)
            {
                var arr = aString.Split(ValueSplitterChar.ToCharArray());
                if(arr.Length >= 5)
                {
                    return arr[4];
                }
            }
            return string.Empty;
        }

        private string GetSecondValue(string aString)
        {
            if (aString.IndexOf(ValueSplitterChar, StringComparison.Ordinal) >= 0)
            {
                var arr = aString.Split(ValueSplitterChar.ToCharArray());
                if (arr.Length >= 2)
                {
                    return arr[1];
                }
            }
            return string.Empty;
        }

        private string ExtractUnit(string aString)
        {
            if (aString.IndexOf(ValueSplitterChar, StringComparison.Ordinal) >= 0)
            {
                var arr = aString.Split(ValueSplitterChar.ToCharArray());
                if (arr.Length >= 5)
                {
                    return arr[3];
                }
            }
            return string.Empty;
        }

        private string GetResourceGroupTimes(RequestGroupTime request)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode getResourceGroupTimes = xmlDoc.CreateElement("GetResourceGroupTimesOutside");

            //****************** Resursgruppen *******************
            XmlNode resourceGroup = xmlDoc.CreateElement("ResourceGroup");
            resourceGroup.InnerText = request.ResourceGroup;
            getResourceGroupTimes.AppendChild(resourceGroup);

            //****************** Fordonstyp *******************
            XmlNode resourceGroupVehicleType = xmlDoc.CreateElement("ResourceGroupVehicleType");
            resourceGroupVehicleType.InnerText = request.VehicleTypeId;
            getResourceGroupTimes.AppendChild(resourceGroupVehicleType);

            //****************** Lastningselement *******************
            XmlNode loadingXml = xmlDoc.CreateElement("Loading");
            //****************** LossningsElement *******************
            XmlNode unLoadingXml = xmlDoc.CreateElement("Unloading");
            //****************** ArtikelElement *******************
            XmlNode item = xmlDoc.CreateElement("Item");
            //****************** ArtikelID *******************
            XmlNode id = xmlDoc.CreateElement("ID");
            id.InnerText = request.ItemId;

            //****************** Kvantiteten *******************
            XmlNode quantity = xmlDoc.CreateElement("Quantity");
            quantity.InnerText = request.Quantity.ToString("0");

            //****************** Torkad *******************
            XmlNode dried = xmlDoc.CreateElement("Dried");
            dried.InnerText = request.Dried.ToString().ToLower();

            item = XmlFnItemData(item, id, dried);
            if(request.Loading)
            {
                loadingXml.AppendChild(item);
                loadingXml.AppendChild(quantity);
                //****************** Lägg till artikelelemnetet *******************
                getResourceGroupTimes.AppendChild(loadingXml);
            }
            if(request.Unloading)
            {
                unLoadingXml.AppendChild(item);
                unLoadingXml.AppendChild(quantity);
                //******************Lägg till artikelelemnetet *******************
                getResourceGroupTimes.AppendChild(unLoadingXml);
            }

            //****************** FrånDatum *******************
            //** Huvudnode From
            XmlNode fromDate = xmlDoc.CreateElement("FromDate");

            //**Childnode Datum
            XmlNode date = xmlDoc.CreateElement("Date");
            fromDate.AppendChild(date);

            //** Från år
            XmlNode dateYear = xmlDoc.CreateElement("Year");
            dateYear.InnerText = request.DateFrom.Year.ToString();
            date.AppendChild(dateYear);

            //** Från Månad
            XmlNode dateMonth = xmlDoc.CreateElement("Month");
            dateMonth.InnerText = request.DateFrom.Month.ToString();
            date.AppendChild(dateMonth);

            //** Från Dag
            XmlNode dateDay = xmlDoc.CreateElement("Day");
            dateDay.InnerText = request.DateFrom.Day.ToString();
            date.AppendChild(dateDay);

            //** Lägg till FrånDatum
            getResourceGroupTimes.AppendChild(fromDate);

            //************** Tilldatum *********************'
            //** Huvudnode TO
            XmlNode toDate = xmlDoc.CreateElement("ToDate");

            //** Childnode Datum
            XmlNode to_date = xmlDoc.CreateElement("Date");
            toDate.AppendChild(to_date);

            //** To år
            XmlNode toDateYear = xmlDoc.CreateElement("Year");
            toDateYear.InnerXml = request.DateTo.Year.ToString();
            to_date.AppendChild(toDateYear);

            //** To Månad
            XmlNode toDateMonth = xmlDoc.CreateElement("Month");
            toDateMonth.InnerXml = request.DateTo.Month.ToString();
            to_date.AppendChild(toDateMonth);

            //** To Dag
            XmlNode toDateDay = xmlDoc.CreateElement("Day");
            toDateDay.InnerXml = request.DateTo.Day.ToString();
            to_date.AppendChild(toDateDay);

            //** Lägg till To Datum
            getResourceGroupTimes.AppendChild(toDate);

            //*** Lägger till hela Roten till dokumentet
            xmlDoc.AppendChild(getResourceGroupTimes);

            var result = _bokaSoap.GetResourceGroupTimesOutside(new GetResourceGroupTimesOutsideRequest() {
                Body = new GetResourceGroupTimesOutsideRequestBody()
                {
                    xml = xmlDoc.InnerXml
                }
            });
            return result != null ? result.Body.GetResourceGroupTimesOutsideResult : string.Empty;
        }

        private XmlNode XmlFnItemData(XmlNode item, XmlNode id, XmlNode dried)
        {
            item.AppendChild(id);
            item.AppendChild(dried);
            return item;
        }

        private ResourceGroupTimes GetTimes(string xml)
        {
            var serializer = new XmlSerializer(typeof(ResourceGroupTimes));
            ResourceGroupTimes result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (ResourceGroupTimes)serializer.Deserialize(reader);
            }
            return result;
        }

        private bool IsEditingEnabledForReservation(double minutesToReservation, bool isInternal, string currentUserId, 
            string rowUserId, string rowCustomerNo)
        {
            var returnValue = false;
            var editLimmitInMinutes = isInternal ? int.Parse(ConfigurationManager.AppSettings["TimeLimmitForInternalEdit"]) : 
                                        int.Parse(ConfigurationManager.AppSettings["TimeLimmitForExternalEdit"]);
            if (!isInternal)
            {
                if (!currentUserId.Equals(rowUserId, StringComparison.CurrentCultureIgnoreCase) &&
                    !currentUserId.Equals(rowCustomerNo, StringComparison.CurrentCultureIgnoreCase)) return false;
                if (minutesToReservation >= editLimmitInMinutes)
                {
                    returnValue = true;
                }
            }
            else
            {
                if(minutesToReservation >= editLimmitInMinutes)
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        public bool ExistBooking(string resourceGroupId, string selectedDate, string article, string qty, string loadunload, string veichleType, string driedUnDried, string customerNo, string searchType, string iONumber)
        {
            bool retVal = false;
            switch(searchType)
            {
                case SearchTypeIoNo:
                    var request = new RequestGroupTime()
                    {
                        ResourceGroup = resourceGroupId,
                        ItemId = GetFirstValue(article),
                        CustomerNo = customerNo,
                        Dried = GetSecondValue(article).Equals("torkad", StringComparison.CurrentCultureIgnoreCase),
                        Quantity = Decimal.Parse(qty.Replace(".", ",")),
                        VehicleTypeId = veichleType,
                        _Loading = true,
                        _Unloading = true,
                        DateFrom = Convert.ToDateTime(selectedDate),
                        Loading = loadunload == "1",
                        Unloading = loadunload == "2"
                    };
                    request.DateTo = request.DateFrom.AddDays(1);
                    var xmlString = GetResourceGroupTimes(request);
                    var resourceGroupTimes = GetTimes(xmlString);
                    var listGroupTime = loadunload.Equals("1") ? resourceGroupTimes.Loading : resourceGroupTimes.Unloading;

                    foreach(var row in listGroupTime)
                    {
                        if(row.Reservation != null)
                        {
                            if(iONumber.Equals(row.Reservation.ReferenceNumber))
                            {
                                retVal = true;
                                break;
                            }
                        }
                    }
                    break;
            }
            return retVal;
        }

        private string GetFirstValue(string aString)
        {
            return aString.Split(ValueSplitterChar.ToCharArray())[0];
        }

        private string GetReferenceType(string s)
        {
            string retVal;
            switch(s)
            {
                case "5":
                    retVal = "PurchaseOrder";
                    break;
                case "6":
                    retVal = "CustomerOrder";
                    break;
                case "7":
                    retVal = "DistributionOrder";
                    break;
                case "8":
                    retVal = "Shipment";
                    break;
                case "9":
                    retVal = "CustomerNumber";
                    break;
                default:
                    retVal = "Default";
                    break;

            }
            return retVal;
        }

        public List<ReservationResultDto> MakeReservation(MakeReservationDto reservationToMake, string customerNo)
        {
            var currentUser = SettingPage.IsInternal ? "Intern" : customerNo;
            var ipAddress = HttpContext.Current.Request.UserHostAddress;
            var transportOrderNo = string.Empty;
            var reminderEmail = false;
            bool verificationEmail = false;
            bool reminderSms = false;
            bool verificationSms = false;
            int reminderMinutesBefore = 0;
            List<ReservationResultDto> svaret = new List<ReservationResultDto>();
            if (string.IsNullOrEmpty(currentUser))
            {
                var anError = new ReservationResultDto()
                {
                    Status = UnKnownErrorStatus,
                    ErrorMessage = "Du är inte inloggad!",
                };
                svaret.Add(anError);
            }
            else
            {
                var changedBy = string.Empty;
                var mobilePhone = !string.IsNullOrEmpty(reservationToMake.MobilePhone) ? RemoveIlegaCharacters(reservationToMake.MobilePhone) : string.Empty;
                var contractNo = ExtractAgrementNo(reservationToMake.Agrement);
                
                var owner = SettingPage.IsInternal ? RemoveIlegaCharacters(reservationToMake.CustomerNo) : customerNo;

                if(!string.IsNullOrEmpty(reservationToMake.ReminderInMinutesBefore))
                {
                    reminderMinutesBefore = Convert.ToInt32(reservationToMake.ReminderInMinutesBefore);
                    reminderEmail = reservationToMake.EmailAddress?.Trim().Length > 0;
                    verificationEmail = reservationToMake.EmailAddress?.Trim().Length > 0;

                    reminderSms = reservationToMake.MobilePhone?.Trim().Length > 0;
                    verificationSms = reservationToMake.MobilePhone?.Trim().Length > 0;
                }
                var itemId = ExtractArticleId(reservationToMake.Item);
                var dried = GetSecondValue(reservationToMake.Item).Equals("torkad", StringComparison.InvariantCultureIgnoreCase);
                var unit = ExtractUnit(reservationToMake.Item);
                if(unit.Equals("kg", StringComparison.InvariantCultureIgnoreCase))
                {
                    reservationToMake.Qty = Convert.ToInt32(decimal.Parse(reservationToMake.Qty.Replace(".", ",")) * 1000).ToString();
                }

                var referenceTypeStr = GetReferenceType(reservationToMake.SearchType);

                var result = MakeReservations(reservationToMake.ResourceId, changedBy, reservationToMake.MobilePhone,
                                              contractNo, reservationToMake.CustomerName,
                                                  reservationToMake.CustomerNo,
                                                  !string.IsNullOrEmpty(reservationToMake.EmailAddress) ? RemoveIlegaCharacters(reservationToMake.EmailAddress) : string.Empty,
                                                  ipAddress,
                                                  reservationToMake.Leveransforsakransnummer,
                                                  !string.IsNullOrEmpty(reservationToMake.LicensePlateNo) ? RemoveIlegaCharacters(reservationToMake.LicensePlateNo) : string.Empty,
                                                  mobilePhone,
                                                  !string.IsNullOrEmpty(reservationToMake.Note) ? RemoveIlegaCharacters(reservationToMake.Note) : string.Empty,
                                                  transportOrderNo,
                                                  owner,
                                                  reminderEmail,
                                                  reminderMinutesBefore,
                                                  reminderSms,
                                                  reservationToMake.Reservations,
                                                  string.Empty,
                                                  reservationToMake.VehicleAssortmentID,
                                                  verificationEmail,
                                                  verificationSms,
                                                  Convert.ToDateTime(reservationToMake.SelectedDate),
                                                  itemId,
                                                  dried,
                                                  reservationToMake.Qty,
                                                  referenceTypeStr,
                                                  reservationToMake.SearchValue,
                                                  reservationToMake.LineNumber
                                                  );

                foreach (var item in result.ReservationList)
                {
                    var anResult = new ReservationResultDto();
                    if (item.ID.Equals("0"))
                    {
                        anResult.Status = UnKnownErrorStatus;
                        anResult.ErrorMessage = "Bokningsfel.";
                    }
                    else
                    {
                        anResult.Status = OkStatus;
                        anResult.ReservationId = item.ID;
                        anResult.ErrorMessage = string.Empty;

                        anResult.StartDate = DateTime.Parse(item.StartTime.Date.Year + "-" + item.StartTime.Date.Month + "-" + item.StartTime.Date.Day).ToString("yyyy-MM-dd");
                        anResult.StartTime = DateTime.Parse(item.StartTime.Time.Hour + ":" + item.StartTime.Time.Minute).ToString("HH:mm");
                        anResult.CustomerName = item.CustomerName;
                        anResult.CustomerNo = item.CustomerNo;

                        anResult.ReferenceType = string.Empty;
                        anResult.ReferenceNumber = string.Empty;
                        anResult.PurchaseOrderLine = 0;
                    }
                    svaret.Add(anResult);
                }
            }
            return svaret;
        }

        private string RemoveIlegaCharacters(string value)
        {
            value = value.Replace("\"\"", "");
            value = value.Replace("\'", "");
            value = value.Replace("\\", "");
            return value;
        }

        private string ExtractAgrementNo(string agrementText)
        {
            if(string.IsNullOrEmpty(agrementText))
            {
                return string.Empty;
            }
            else
            {
                var arr = agrementText.Trim().Split("-".ToArray());
                if(arr.Length > 1)
                {
                    return arr[0];
                }
            }
            return string.Empty;
        }

        private MakeReservationResult MakeReservations(string resourceGroup, string changedBy, string contactTelephone, 
                                        string contractNo, string customerName, string customerNo, 
                                        string emailAddress, string ipAddress, string leveransforsakransnr,
                                        string licensePlateNo, string mobileNo,
                                        string note, string transportOrderNo, string owner, bool reminderEmail,
                                        int reminderMinutesBefore, bool reminderSms, IEnumerable<TimeReservationDto> reservations,
                                        string speditorNo, string vehicleAssortmentId, bool verificationEmail,
                                        bool verificationSms, DateTime startTime, string itemId, bool dried, string quantity,
                                        string referenceType, string referenceNumber, int? purchaseOrderLine)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            XmlNode xmlMakeReservation = xmlDoc.CreateElement("MakeReservation");

            //****************** Resursgruppen *******************
            XmlNode xmlResourceGroup = xmlDoc.CreateElement("ResourceGroup");
            xmlResourceGroup.InnerText = resourceGroup;
            xmlMakeReservation.AppendChild(xmlResourceGroup);

            //** Loopa igenom Bokningar
            foreach(var item in reservations)
            {
                //****************** Reservationen Huvudnode*******************
                XmlNode xmlReservation = xmlDoc.CreateElement("Reservation");

                //******************FrånTid * ******************
                //** Huvudnode Starttid
                XmlNode xmlStartTime = xmlDoc.CreateElement("StartTime");

                //** Childnode Datum
                XmlNode xmlDate = xmlDoc.CreateElement("Date");

                //** Från år
                XmlNode xmlDateYear = xmlDoc.CreateElement("Year");
                xmlDateYear.InnerXml = startTime.Year.ToString();
                xmlDate.AppendChild(xmlDateYear);

                //** Från Månad
                XmlNode xmlDateMonth = xmlDoc.CreateElement("Month");
                xmlDateMonth.InnerXml = startTime.Month.ToString();
                xmlDate.AppendChild(xmlDateMonth);

                //** Från Dag
                XmlNode xmlDateDay = xmlDoc.CreateElement("Day");
                xmlDateDay.InnerXml = startTime.Day.ToString();
                xmlDate.AppendChild(xmlDateDay);

                //** Lägger till date till Startdate
                xmlStartTime.AppendChild(xmlDate);

                //*******************
                //** Childnode Tid
                XmlNode xmlTime = xmlDoc.CreateElement("Time");
                xmlStartTime.AppendChild(xmlTime);

                //** Från Timme
                XmlNode xmlTimeHour = xmlDoc.CreateElement("Hour");
                xmlTimeHour.InnerXml = Convert.ToDateTime(item.FromTime).Hour.ToString();
                xmlTime.AppendChild(xmlTimeHour);

                //** Från Minut
                XmlNode xmlTimeMinute = xmlDoc.CreateElement("Minute");
                xmlTimeMinute.InnerXml = Convert.ToDateTime(item.FromTime).Minute.ToString();
                xmlTime.AppendChild(xmlTimeMinute);

                //** Lägger till Tid till Startdate
                xmlStartTime.AppendChild(xmlTime);

                //** Lägg till FrånDatum
                xmlReservation.AppendChild(xmlStartTime);


                //****************** Resurs ******************
                XmlNode xmlResource = xmlDoc.CreateElement("Resource");
                xmlResource.InnerText = item.ResourceId;
                xmlReservation.AppendChild(xmlResource);

                //****************** Speditör ******************
                XmlNode xmlSpeditorNo = xmlDoc.CreateElement("SpeditorNo");
                xmlSpeditorNo.InnerText = speditorNo;
                xmlReservation.AppendChild(xmlSpeditorNo);

                //****************** Fordonstyp ******************
                XmlNode xmlVehicleAssortmentId = xmlDoc.CreateElement("VehicleAssortmentID");
                xmlVehicleAssortmentId.InnerText = vehicleAssortmentId;
                xmlReservation.AppendChild(xmlVehicleAssortmentId);

                //****************** Regnr ******************
                XmlNode xmlLicensePlateNo = xmlDoc.CreateElement("LicensePlateNo");
                xmlLicensePlateNo.InnerText = licensePlateNo;
                xmlReservation.AppendChild(xmlLicensePlateNo);

                //****************** Kontaktnr ******************
                XmlNode xmlContactTelephone = xmlDoc.CreateElement("ContactTelephone");
                xmlContactTelephone.InnerText = contactTelephone;
                xmlReservation.AppendChild(xmlContactTelephone);

                //****************** Leveransforsakransnr ******************
                XmlNode xmlLeveransforsakransnr = xmlDoc.CreateElement("Leveransforsakransnr");
                xmlLeveransforsakransnr.InnerText = leveransforsakransnr;
                xmlReservation.AppendChild(xmlLeveransforsakransnr);

                //****************** Lasta ******************
                XmlNode xmlLoading = xmlDoc.CreateElement("Loading");
                xmlLoading.InnerText = item.Loading;
                xmlReservation.AppendChild(xmlLoading);

                //****************** Lossa ******************
                XmlNode xmlUnloading = xmlDoc.CreateElement("Unloading");
                xmlUnloading.InnerText = item.Unloading;
                xmlReservation.AppendChild(xmlUnloading);

                //****************** Kundnr ******************
                XmlNode xmlCustomerNo = xmlDoc.CreateElement("CustomerNo");
                xmlCustomerNo.InnerText = customerNo;
                xmlReservation.AppendChild(xmlCustomerNo);

                //****************** Kundnamn ******************
                XmlNode xmlCustomerName = xmlDoc.CreateElement("CustomerName");
                xmlCustomerName.InnerText = customerName;
                xmlReservation.AppendChild(xmlCustomerName);

                //****************** Övrigt ******************
                XmlNode xmlNote = xmlDoc.CreateElement("Note");
                xmlNote.InnerText = note;
                xmlReservation.AppendChild(xmlNote);

                //****************** TransportOrderNo ******************
                XmlNode xmlTransportOrderNo = xmlDoc.CreateElement("TransportOrderNo");
                xmlTransportOrderNo.InnerText = transportOrderNo;
                xmlReservation.AppendChild(xmlTransportOrderNo);

                //****************** ContractNo ******************
                XmlNode xmlContractNo = xmlDoc.CreateElement("ContractNo");
                xmlContractNo.InnerText = contractNo;
                xmlReservation.AppendChild(xmlContractNo);

                //****************** ipnr ******************
                XmlNode xmlIpAddress = xmlDoc.CreateElement("IPAddress");
                xmlIpAddress.InnerText = ipAddress;
                xmlReservation.AppendChild(xmlIpAddress);

                //****************** Mobil ******************
                XmlNode xmlMobileNo = xmlDoc.CreateElement("MobileNo");
                xmlMobileNo.InnerText = mobileNo;
                xmlReservation.AppendChild(xmlMobileNo);

                //****************** Email ******************
                XmlNode xmlEmailAddress = xmlDoc.CreateElement("EmailAddress");
                xmlEmailAddress.InnerText = emailAddress;
                xmlReservation.AppendChild(xmlEmailAddress);

                //****************** ReminderSMS ******************
                XmlNode xmlReminderSms = xmlDoc.CreateElement("ReminderSMS");
                xmlReminderSms.InnerText = reminderSms.ToString().ToLower();
                xmlReservation.AppendChild(xmlReminderSms);


                //****************** ReminderEmail ******************
                XmlNode xmlReminderEmail = xmlDoc.CreateElement("ReminderEmail");
                xmlReminderEmail.InnerText = reminderEmail.ToString().ToLower();
                xmlReservation.AppendChild(xmlReminderEmail);

                //****************** ReminderEmail ******************
                XmlNode xmlReminderMinutesBefore = xmlDoc.CreateElement("ReminderMinutesBefore");
                xmlReminderMinutesBefore.InnerText = reminderMinutesBefore.ToString();
                xmlReservation.AppendChild(xmlReminderMinutesBefore);

                //****************** VerificationSMS ******************
                XmlNode xmlVerificationSms = xmlDoc.CreateElement("VerificationSMS");
                xmlVerificationSms.InnerText = verificationSms.ToString().ToLower();
                xmlReservation.AppendChild(xmlVerificationSms);

                //****************** VerificationEmail ******************
                XmlNode xmlVerificationEmail = xmlDoc.CreateElement("VerificationEmail");
                xmlVerificationEmail.InnerText = verificationEmail.ToString().ToLower();
                xmlReservation.AppendChild(xmlVerificationEmail);

                //****************** Ägare ******************
                XmlNode xmlOwner = xmlDoc.CreateElement("Owner");
                xmlOwner.InnerText = owner;
                xmlReservation.AppendChild(xmlOwner);

                //****************** Ändrad av ******************
                XmlNode xmlChangedBy = xmlDoc.CreateElement("ChangedBy");
                xmlChangedBy.InnerText = changedBy;
                xmlReservation.AppendChild(xmlChangedBy);
                if (referenceType != "CustomerNumber")
                {
                    //** ReferenceType ******************
                    XmlNode xmlReferenceType = xmlDoc.CreateElement("ReferenceType");
                    xmlReferenceType.InnerText = referenceType;
                    xmlReservation.AppendChild(xmlReferenceType);

                    //** ReferenceNumber ******************
                    XmlNode xmlReferenceNumber = xmlDoc.CreateElement("ReferenceNumber");
                    xmlReferenceNumber.InnerText = referenceNumber;
                    xmlReservation.AppendChild(xmlReferenceNumber);
                }
                //** PurchaseOrderLine ******************
                XmlNode xmlPurchaseOrderLine = xmlDoc.CreateElement("PurchaseOrderLine");
                xmlPurchaseOrderLine.InnerText = purchaseOrderLine.ToString();
                xmlReservation.AppendChild(xmlPurchaseOrderLine);

                //****************** Reserverade Artiklar Huvudnode ******************
                XmlNode xmlReservationItems = xmlDoc.CreateElement("ReservationItems");

                //****************** Reserverade Artiklar Childnode ******************
                XmlNode xmlReservationItem = xmlDoc.CreateElement("ReservationItem");

                //** ArtikelID ******************
                XmlNode xmlItemId = xmlDoc.CreateElement("ItemID");
                xmlItemId.InnerText = itemId;
                xmlReservationItem.AppendChild(xmlItemId);

                //** Torkad ******************
                XmlNode xmlDried = xmlDoc.CreateElement("Dried");
                xmlDried.InnerText = dried.ToString().ToLower();
                xmlReservationItem.AppendChild(xmlDried);

                //** Kvantitet ******************
                XmlNode xmlQuantity = xmlDoc.CreateElement("Quantity");
                xmlQuantity.InnerText = quantity;
                xmlReservationItem.AppendChild(xmlQuantity);

                //** LÄgger till reserverade item
                xmlReservationItems.AppendChild(xmlReservationItem);
                //** LÄgger till reserverade items
                xmlReservation.AppendChild(xmlReservationItems);

                //** Slutför strängen lägger till reservationen
                xmlMakeReservation.AppendChild(xmlReservation);
            }
            xmlDoc.AppendChild(xmlMakeReservation);

            var xmlResult = _bokaSoap.MakeReservation(new MakeReservationRequest() {
                Body = new MakeReservationRequestBody() {
                    xml = xmlDoc.InnerXml
                }
            });

            var result = new MakeReservationResult();
            if (!string.IsNullOrWhiteSpace(xmlResult?.Body?.MakeReservationResult))
            {
                result = GetMakeReservation(xmlResult.Body.MakeReservationResult);
            }

            return result;
        }

        private MakeReservationResult GetMakeReservation(string xml)
        {
            var serializer = new XmlSerializer(typeof(MakeReservationResult));
            MakeReservationResult result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (MakeReservationResult)serializer.Deserialize(reader);
            }
            return result;
        }

        public List<DeleReservationResultDto> DeleteReservation(string reservationId, string owner, string customerNo, DateTime dateRegistered)
        {
            var isInternal = SettingPage.IsInternal;
            var answer = new List<DeleReservationResultDto>();
            var minutesToReservation = (dateRegistered - DateTime.Now).TotalMinutes;

            if (!IsEditingEnabledForReservation(minutesToReservation, isInternal, customerNo, owner, customerNo))
                return answer;
            var result = DeleteReservation(reservationId);
            if (result == null) return answer;
            if(result.Result?.Error?.Count > 0)
            {
                foreach (var err in result.Result.Error)
                {
                    var anAnswer = new DeleReservationResultDto()
                    {
                        Status = UnKnownErrorStatus,
                        Message = err.Description
                    };
                    answer.Add(anAnswer);
                }
            }
            else
            {
                var anAnswer = new DeleReservationResultDto()
                {
                    Status = OkStatus,
                    Message = "Bokningen är raderad."
                };
                answer.Add(anAnswer);
            }

            return answer;
        }

        private DeleteReservationResult DeleteReservation(string reservationId)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            XmlNode xmlDeleteReservation = xmlDoc.CreateElement("DeleteReservation");

            XmlNode xmlId = xmlDoc.CreateElement("ID");
            xmlId.InnerText = reservationId;
            xmlDeleteReservation.AppendChild(xmlId);

            xmlDoc.AppendChild(xmlDeleteReservation);

            var result = _bokaSoap.DeleteReservation(new DeleteReservationRequest() {
                Body = new DeleteReservationRequestBody()
                {
                    xml = xmlDoc.InnerXml
                }
            });
            DeleteReservationResult dataValue = new DeleteReservationResult();
            if (!string.IsNullOrWhiteSpace(result?.Body?.DeleteReservationResult))
            {
                var serializer = new XmlSerializer(typeof(DeleteReservationResult));
                using (TextReader reader = new StringReader(result.Body.DeleteReservationResult))
                {
                    dataValue = (DeleteReservationResult)serializer.Deserialize(reader);
                }
            }

            return dataValue;
        }

        public List<ReservationResultDto> UpdateReservation(UpdateReservationDto reservationToUpdate, string customerNo)
        {
            var isInternal = SettingPage.IsInternal;

            var svaret = new List<ReservationResultDto>();

            var minutesToReservation = (reservationToUpdate.DateRegistered - DateTime.Now).TotalMinutes;
            if (IsEditingEnabledForReservation(minutesToReservation, isInternal, customerNo, reservationToUpdate.Owner, reservationToUpdate.OldCustomerNo))
            {
                var reservationId = reservationToUpdate.ReservationId;
                var changedBy = string.Empty;
                var reminderSms = !string.IsNullOrEmpty(reservationToUpdate.ReminderInMinutesBefore)
                    && !string.IsNullOrEmpty(reservationToUpdate.MobilePhone)
                    && reservationToUpdate.MobilePhone.Trim().Length > 0;
                var reminderEmail = !string.IsNullOrEmpty(reservationToUpdate.ReminderInMinutesBefore)
                    && !string.IsNullOrEmpty(reservationToUpdate.EmailAddress)
                    && reservationToUpdate.EmailAddress.Trim().Length > 0;
                var reminderMinutesBefore = !string.IsNullOrEmpty(reservationToUpdate.ReminderInMinutesBefore) ? Convert.ToInt32(reservationToUpdate.ReminderInMinutesBefore) : 0;


                var ipAddress = HttpContext.Current.Request.UserHostAddress;
                var answer = UpdateReservations(reservationId, changedBy, ipAddress,
                                                !string.IsNullOrEmpty(reservationToUpdate.SpeditorNo) ? RemoveIlegaCharacters(reservationToUpdate.SpeditorNo) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.LicensePlateNo) ? RemoveIlegaCharacters(reservationToUpdate.LicensePlateNo) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.MobilePhone) ? RemoveIlegaCharacters(reservationToUpdate.MobilePhone) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.Leveransforsakransnummer) ? RemoveIlegaCharacters(reservationToUpdate.Leveransforsakransnummer) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.NewCustomerNo) ? RemoveIlegaCharacters(reservationToUpdate.NewCustomerNo) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.NewCustomerName) ? RemoveIlegaCharacters(reservationToUpdate.NewCustomerName) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.Note) ? RemoveIlegaCharacters(reservationToUpdate.Note) : string.Empty,
                                                string.Empty, string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.MobilePhone) ? RemoveIlegaCharacters(reservationToUpdate.MobilePhone) : string.Empty,
                                                !string.IsNullOrEmpty(reservationToUpdate.EmailAddress) ? RemoveIlegaCharacters(reservationToUpdate.EmailAddress) : string.Empty,
                                                reminderSms, reminderEmail, reminderMinutesBefore.ToString(), string.Empty, string.Empty, reservationToUpdate.LineNumber
                                                );

                var noErrorsFound = true;
                if (answer?.Result == null) return svaret;
                foreach (var rowError in answer.Result.Error)
                {
                    var anError = new ReservationResultDto()
                    {
                        Status = UnKnownErrorStatus,
                        ErrorMessage = $"Fel vid uppdatering Fel: {rowError.Description}."
                    };
                    svaret.Add(anError);
                    noErrorsFound = false;
                }

                if (!noErrorsFound) return svaret;
                var anOk = new ReservationResultDto()
                {
                    Status = OkStatus,
                    Message = "Uppdateringen är sparad."
                };
                svaret.Add(anOk);
            }
            else
            {
                var anError = new ReservationResultDto() {
                    Status = AccessErrorStatus,
                    ErrorMessage = "Du saknar behörighet att uppdatera denna bokning."
                };
                svaret.Add(anError);
            }
            return svaret;
        }

        private ChangeReservationResult UpdateReservations(string reservationId, string changedBy, string ipAddress,
                                        string speditorNo, string licensePlateNo, string contactTelephone,
                                        string leveransforsakransnr, string customerNo, string customerName,
                                        string note, string transportOrderNo, string contractNo,
                                        string mobileNo, string emailAddress, bool reminderSms, 
                                        bool reminderEmail, string reminderMinutesBefore, string referenceType, 
                                        string referenceNumber, int? purchaseOrderLine)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            //** HuvudNode
            XmlNode xmlChangeReservation = xmlDoc.CreateElement("ChangeReservation");

            // * ReservationID
            XmlNode xmlReservationId = xmlDoc.CreateElement("ReservationID");
            xmlReservationId.InnerText = reservationId;
            xmlChangeReservation.AppendChild(xmlReservationId);

            // * ChangedBy
            XmlNode xmlChangedBy = xmlDoc.CreateElement("ChangedBy");
            xmlChangedBy.InnerText = changedBy.Trim();
            xmlChangeReservation.AppendChild(xmlChangedBy);

            //****************** ipnr ******************
            XmlNode xmlIpAddress = xmlDoc.CreateElement("IPAddress");
            xmlIpAddress.InnerText = ipAddress.ToLower();
            xmlChangeReservation.AppendChild(xmlIpAddress);

            //****************** Ändrad av ******************

            XmlNode xmlChangesNode = xmlDoc.CreateElement("Changes");

            // * SpeditorNo
            XmlNode xmlSpeditorNo = xmlDoc.CreateElement("SpeditorNo");
            xmlSpeditorNo.InnerText = speditorNo.Trim();
            xmlChangesNode.AppendChild(xmlSpeditorNo);

            // * LicensePlateNo
            XmlNode xmlLicensePlateNo = xmlDoc.CreateElement("LicensePlateNo");
            xmlLicensePlateNo.InnerText = licensePlateNo.Trim();
            xmlChangesNode.AppendChild(xmlLicensePlateNo);

            // * ContactTelephone
            XmlNode xmlContactTelephone = xmlDoc.CreateElement("ContactTelephone");
            xmlContactTelephone.InnerText = contactTelephone.Trim();
            xmlChangesNode.AppendChild(xmlContactTelephone);

            // * Leveransforsakransnr
            XmlNode xmlLeveransforsakransnr = xmlDoc.CreateElement("Leveransforsakransnr");
            xmlLeveransforsakransnr.InnerText = leveransforsakransnr.Trim();
            xmlChangesNode.AppendChild(xmlLeveransforsakransnr);

            // * CustomerNo
            XmlNode xmlCustomerNo = xmlDoc.CreateElement("CustomerNo");
            xmlCustomerNo.InnerText = customerNo.Trim();
            xmlChangesNode.AppendChild(xmlCustomerNo);

            // * CustomerName
            XmlNode xmlCustomerName = xmlDoc.CreateElement("CustomerName");
            xmlCustomerName.InnerText = customerName.Trim();
            xmlChangesNode.AppendChild(xmlCustomerName);

            // * Note
            XmlNode xmlNote = xmlDoc.CreateElement("Note");
            xmlNote.InnerText = note.Trim();
            xmlChangesNode.AppendChild(xmlNote);

            // * TransportOrderNo
            XmlNode xmlTransportOrderNo = xmlDoc.CreateElement("TransportOrderNo");
            xmlTransportOrderNo.InnerText = transportOrderNo.Trim();
            xmlChangesNode.AppendChild(xmlTransportOrderNo);

            // * ContractNo
            XmlNode xmlContractNo = xmlDoc.CreateElement("ContractNo");
            xmlContractNo.InnerText = contractNo.Trim();
            xmlChangesNode.AppendChild(xmlContractNo);

            //* MobileNo
            XmlNode xmlMobileNo = xmlDoc.CreateElement("MobileNo");
            xmlMobileNo.InnerText = mobileNo.Trim();
            xmlChangesNode.AppendChild(xmlMobileNo);

            // * EmailAddres
            XmlNode xmlEmailAddress = xmlDoc.CreateElement("EmailAddress");
            xmlEmailAddress.InnerText = emailAddress.Trim();
            xmlChangesNode.AppendChild(xmlEmailAddress);

            // * ReminderSMS
            XmlNode xmlReminderSms = xmlDoc.CreateElement("ReminderSMS");
            xmlReminderSms.InnerText = reminderSms.ToString().ToLower();
            xmlChangesNode.AppendChild(xmlReminderSms);

            // * ReminderEmail
            XmlNode xmlReminderEmail = xmlDoc.CreateElement("ReminderEmail");
            xmlReminderEmail.InnerText = reminderEmail.ToString().ToLower();
            xmlChangesNode.AppendChild(xmlReminderEmail);

            // * ReminderMinutesBefore
            XmlNode xmlReminderMinutesBefore = xmlDoc.CreateElement("ReminderMinutesBefore");
            xmlReminderMinutesBefore.InnerText = reminderMinutesBefore;
            xmlChangesNode.AppendChild(xmlReminderMinutesBefore);

            // * ReferenceType
            if ((referenceType.Trim().Length > 0))
            {
                XmlNode xmlReferenceType = xmlDoc.CreateElement("ReferenceType");
                xmlReferenceType.InnerText = referenceType.Trim();
                xmlChangesNode.AppendChild(xmlReferenceType);
            }
            // * ReferenceNumber
            if ((referenceNumber.Trim().Length > 0))
            {
                XmlNode xmlReferenceNumber = xmlDoc.CreateElement("ReferenceNumber");
                xmlReferenceNumber.InnerText = referenceNumber.Trim();
                xmlChangesNode.AppendChild(xmlReferenceNumber);
            }
            // * PurchaseOrderLine
            if ((purchaseOrderLine.HasValue))
            {
                XmlNode xmlPurchaseOrderLine = xmlDoc.CreateElement("PurchaseOrderLine");
                xmlPurchaseOrderLine.InnerText = purchaseOrderLine.Value.ToString();
                xmlChangesNode.AppendChild(xmlPurchaseOrderLine);
            }
            xmlChangeReservation.AppendChild(xmlChangesNode);

            xmlDoc.AppendChild(xmlChangeReservation);

            var updateReservationResult = _bokaSoap.ChangeReservation(new ChangeReservationRequest()
            {
                Body = new ChangeReservationRequestBody()
                {
                    xml = xmlDoc.InnerXml
                }
            });
            ChangeReservationResult dataValue = new ChangeReservationResult();
            if (!string.IsNullOrWhiteSpace(updateReservationResult?.Body?.ChangeReservationResult))
            {
                var serializer = new XmlSerializer(typeof(ChangeReservationResult));
                using (TextReader reader = new StringReader(updateReservationResult.Body.ChangeReservationResult))
                {
                    dataValue = (ChangeReservationResult)serializer.Deserialize(reader);
                }
            }

            return dataValue;
        }

        public List<SearchBookingsDto> SearchBookings(RequestSearchBookings requestSearch)
        {
            var isInternal = SettingPage.IsInternal;
            var result = new List<SearchBookingsDto>();
            var owner = isInternal ? string.Empty : requestSearch.CustomerNo;
            var resultList = SearchListnings(requestSearch, owner);
            if (resultList?.Reservation?.Count > 0)
            {
                foreach (var row in resultList.Reservation)
                {
                    var item = new SearchBookingsDto();
                    var eventStartDate = row.EventStartDate;
                    var eventEndDate = row.EventEndDate;
                    DateTime? regArrivalDate = null;
                    if (eventStartDate.HasValue)
                    {
                        item.FromDate = eventStartDate.Value.ToString("yyyy-MM-dd");
                        item.FromTime = $"{eventStartDate.Value: HH:mm}";
                    }
                    if (eventEndDate.HasValue)
                    {
                        item.ToDate = eventEndDate.Value.ToString("yyyy-MM-dd");
                        item.ToTime = $"{eventEndDate.Value: HH:mm}";
                    }
                    if (regArrivalDate.HasValue)
                    {
                        item.ArrivalDate = regArrivalDate.Value.ToShortDateString();
                        item.ArrivalTime = regArrivalDate.Value.ToShortTimeString();
                    }
                    else
                    {
                        item.ArrivalDate = string.Empty;
                        item.ArrivalTime = string.Empty;
                    }

                    if (regArrivalDate.HasValue && eventStartDate.HasValue)
                    {
                        var ts = regArrivalDate - eventStartDate;
                        item.ArrivalTimeDifference = ts.Value.TotalMinutes.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        item.ArrivalTimeDifference = string.Empty;
                    }

                    item.ReservationId = row.Id;
                    item.ResourceId = row.Resource;
                    item.ResourceGroupId = row.ResourceGroup;
                    item.ResourceGroupName = row.ResourceGroupName;
                    item.ResourceName = row.ResourceName;
                    item.SpeditorNo = row.SpeditorNo;
                    item.VehicleAssortmentId = row.VehicleAssortmenId;
                    item.VehicleTypeName = row.VehicleTypeName;
                    item.LicensePlateNo = row.LicensePlateNo;
                    item.Leveransforsakransnr = row.Leveransforsakransnr;
                    item.Loading = row.Loading.ToLower() == "true";
                    item.Unloading = row.Unloading.ToLower() == "true";
                    item.CustomerNo = row.CustomerNo;
                    item.CustomerName = row.CustomerName;
                    item.Note = row.Note;
                    item.TransportOrderNo = row.TransportOrderNo;
                    item.ContractNo = row.ContractNo;
                    item.MobileNo = row.MobileNo;
                    item.EmailAddress = row.EmailAddress;
                    item.ReminderSMS = row.ReminderSms.ToLower() == "true";
                    item.ReminderEmail = row.ReminderEmail.ToLower() == "true";
                    item.ReminderMinutesBefore = int.Parse(row.ReminderMinutesBefore);
                    item.VerificationSMS = row.VerificationSms.ToLower() == "true";
                    item.VerificationEmail = row.VerificationEmail.ToLower() == "true";
                    item.Owner = row.Owner;

                    var dateComp = Convert.ToDateTime($"{item.FromDate} {item.FromTime}");
                    var minutesToReservation = (dateComp - DateTime.Now).TotalMinutes;

                    if (IsEditingEnabledForReservation(minutesToReservation, isInternal, requestSearch.CustomerNo, item.Owner.Trim(), item.CustomerNo))
                    {
                        item.UserCanChange = true;
                    }
                    item.ToolTip = BuildToolTip(isInternal, requestSearch.CustomerNo, row, regArrivalDate);

                    foreach (var resvation in row.ReservationItems.ReservationItem)
                    {
                        item.ItemId = resvation.ItemID;
                        item.ItemName = resvation.ItemName;
                        item.ItemNo = resvation.ItemNo;
                        item.Dried = resvation.Dried.ToLower() == "true";
                        item.Quantity = resvation.Quantity.ToString(CultureInfo.InvariantCulture);
                        item.UnitName = resvation.UnitName;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        private string BuildToolTip(bool isInternal, string currentUser, ReservationSearchBooking row, DateTime? regArrivalDate)
        {
            if (isInternal)
            {
                return ConstructTooTipString(row, regArrivalDate);
            }
            return row.CustomerNo.Equals(currentUser, StringComparison.InvariantCultureIgnoreCase) ? ConstructTooTipString(row, regArrivalDate) : "Tiden är redan bokad.";
        }

        private string ConstructTooTipString(ReservationSearchBooking row, DateTime? regArrivalDate)
        {
            var sb = new StringBuilder();
            sb.Append($"BokningsId: {row.Id}");
            sb.Append(string.Format("{1}Kund nr: {0}", row.CustomerNo.Trim(), Environment.NewLine));
            sb.Append(string.Format("{1}Kund: {0}", row.CustomerName.Trim(), Environment.NewLine));
            sb.Append(string.Format("{1}Leveransförsäkransnr: {0}", row.Leveransforsakransnr, Environment.NewLine));
            sb.Append(string.Format("{1}Regnr: {0}", row.LicensePlateNo.Trim(), Environment.NewLine));
            sb.Append(string.Format("{1}Mobilnr: {0}", row.MobileNo.Trim(), Environment.NewLine));
            sb.Append(string.Format("{1}Epost: {0}", row.EmailAddress.Trim(), Environment.NewLine));
            sb.Append(string.Format("{1}Påminnelse SMS: {0}", row.ReminderSms.ToLower() == "true" ? "Ja" : "Nej", Environment.NewLine));
            sb.Append(string.Format("{1}Påminnelse epost: {0}", row.ReminderEmail.ToLower() == "true" ? "Ja" : "Nej", Environment.NewLine));
            sb.Append(string.Format("{1}Bokad av: {0}", row.Owner, System.Environment.NewLine));

            if (regArrivalDate.HasValue)
            {
                sb.Append(string.Format("{1}Ankomm: {0}", regArrivalDate.Value.ToShortTimeString(), Environment.NewLine));
            }
            return sb.ToString();
        }

        private ResponseSearchBookings SearchListnings(RequestSearchBookings requestSearch, string owner)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode query = xmlDoc.CreateElement("GetReservationList");
            XmlNode conditionsNode = xmlDoc.CreateElement("Conditions");
            
            //From date
            XmlNode fromDateNode = xmlDoc.CreateElement("FromDate");
            XmlNode fromDate = xmlDoc.CreateElement("Date");
            XmlNode fromTime = xmlDoc.CreateElement("Time");

            var fromTimeValue = Convert.ToDateTime(requestSearch.FromDate);
            fromDate.AppendChild(CreateNode(xmlDoc.CreateElement("Year"), fromTimeValue.Year.ToString()));
            fromDate.AppendChild(CreateNode(xmlDoc.CreateElement("Month"), $"{fromTimeValue: MM}"));
            fromDate.AppendChild(CreateNode(xmlDoc.CreateElement("Day"), $"{fromTimeValue: dd}"));
            fromDateNode.AppendChild(fromDate);

            fromTime.AppendChild(CreateNode(xmlDoc.CreateElement("Hour"), "00"));
            fromTime.AppendChild(CreateNode(xmlDoc.CreateElement("Minute"), "00"));
            fromDateNode.AppendChild(fromTime);

            query.AppendChild(fromDateNode);
            //To Date
            XmlNode toDateNode = xmlDoc.CreateElement("ToDate");
            XmlNode toDate = xmlDoc.CreateElement("Date");
            XmlNode toTime = xmlDoc.CreateElement("Time");

            var toTimeValue = Convert.ToDateTime(requestSearch.ToDate);
            toDate.AppendChild(CreateNode(xmlDoc.CreateElement("Year"), toTimeValue.Year.ToString()));
            toDate.AppendChild(CreateNode(xmlDoc.CreateElement("Month"), $"{toTimeValue: MM}"));
            toDate.AppendChild(CreateNode(xmlDoc.CreateElement("Day"), $"{toTimeValue: dd}"));
            toDateNode.AppendChild(toDate);

            toTime.AppendChild(CreateNode(xmlDoc.CreateElement("Hour"), "23"));
            toTime.AppendChild(CreateNode(xmlDoc.CreateElement("Minute"), "59"));
            toDateNode.AppendChild(toTime);

            query.AppendChild(toDateNode);

            if (!string.IsNullOrWhiteSpace(requestSearch.RegNo))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("LicensePlateNo"), requestSearch.RegNo.Trim()));

            if (!string.IsNullOrWhiteSpace(requestSearch.CustomerNo))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("CustomerNo"), requestSearch.CustomerNo.Trim()));

            if (!string.IsNullOrWhiteSpace(owner))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("Owner"), owner.Trim()));

            if (!string.IsNullOrWhiteSpace(requestSearch.ResourceGroupId))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("ResourceGroupID"), requestSearch.ResourceGroupId.Trim()));

            if (!string.IsNullOrWhiteSpace(requestSearch.ReferenceType))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("ReferenceType"), requestSearch.ReferenceType.Trim()));

            if (!string.IsNullOrWhiteSpace(requestSearch.ReferenceNumber))
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("ReferenceNumber"), requestSearch.ReferenceNumber.Trim()));

            if (requestSearch.PurchseOrderLine.HasValue)
                conditionsNode.AppendChild(CreateNode(xmlDoc.CreateElement("ReferenceNumber"), requestSearch.PurchseOrderLine.Value.ToString()));

            query.AppendChild(conditionsNode);
            xmlDoc.AppendChild(query);

            var answer = _bokaSoap.GetReservationList(new GetReservationListRequest
            {
                Body = new GetReservationListRequestBody
                {
                    xml = xmlDoc.InnerXml
                }
            });

            ResponseSearchBookings dataValue = new ResponseSearchBookings();
            if (!string.IsNullOrWhiteSpace(answer?.Body?.GetReservationListResult))
            {
                var serializer = new XmlSerializer(typeof(ResponseSearchBookings));
                using (TextReader reader = new StringReader(answer.Body.GetReservationListResult))
                {
                    dataValue = (ResponseSearchBookings)serializer.Deserialize(reader);
                }
            }
            return dataValue;
        }

        private XmlNode CreateNode(XmlNode aNode, string nodeValue)
        {
            aNode.InnerText = nodeValue.Trim();
            return aNode;
        }
    }
}
