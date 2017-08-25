using System;
using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.DataModels.DeliveryAssuranceDtos;

namespace Gro.ViewModels.Pages.AppPages.DeliveryAssurance
{
    public class DeliveryAssuranceViewModel : PageViewModel<DeliveryAssuranceListPage>
    {
        public DeliveryAssuranceViewModel(DeliveryAssuranceListPage currentPage) : base(currentPage)
        {
            DeliveryAssurance = new DeliveryAssuranceDetail();
        }

        public DeliveryAssuranceDetail DeliveryAssurance { get; set; }
        public List<KeyValuePair<string, string>> LorryTypes { get; set; }
        public List<KeyValuePair<string, string>> DeliveryAddresses { get; set; }
        public List<KeyValuePair<string, string>> DepaAvtals { get; set; }
        public List<KeyValuePair<string, string>> WarehouseList { get; set; }
        public List<KeyValuePair<string,string>> DeliveryValues { get; set; }
        public List<KeyValuePair<string,string>> M3TorkatValues { get; set; }
        public List<KeyValuePair<string,string>> M3RedValues { get; set; }
        public List<KeyValuePair<string,string>> M3StraforkValues { get; set; }
        public List<KeyValuePair<string, string>> M3SlamValues { get; set; }
        public List<KeyValuePair<string, string>> HarvestYears { get; set; }
        public List<KeyValuePair<string,string>>  Articles { get; set; }
        public bool IsInternal { get; set; }
        public bool IsNew { get; set; }

        public bool EnabledLorryType { get; set; }
        public bool EnabledWarehouse { get; set; }
        public bool EnalbedDeliveryDate { get; set; }
        public string DeliveryDateCssClass { get; set; }

    }

    public class DeliveryAssuranceDetail
    {
        public string CustomerName { get; set; }
        public string IONumber { get; set; }
        public string CustomerNumber { get; set; }
        //CPT or CPX
        public string TermAndCondition { get; set; }
        //0 or 1
        public string Gardshamtning { get; set; }
        public string LorryType { get; set; }
        public string DeliveryType { get; set; }
        public string Item { get; set; }
        public string Sort { get; set; }
        public string DeliveryAddress { get; set; }
        public string RequestDate { get; set; }
        public string DeliveryDate { get; set; }
        public string ItemName { get; set; }
        public string Article { get; set; }
        public double Quantity { get; set; }
        public string Slam { get; set; }
        public string Straforkortat { get; set; }
        public string Torkat { get; set; }
        public string Red { get; set; }
        public string OtherInfo { get; set; }
        public string HarvestYear { get; set; }
        public string Warehouse { get; set; }
        public string DepaAvtal { get; set; }
        public string OrderType { get; set; }
        public int LineNumber { get; set; }
        public string Action { get; set; }
        public string CurrentUrl { get; set; }
        public int Status { get; set; }

        public string EnabledWarehouse { get; set; }

    }
}