using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.DataModels.DeliveryAssuranceDtos;
using Gro.Helpers;

namespace Gro.ViewModels.Pages.AppPages.DeliveryAssurance
{
    public class DeliveryAssuranceListViewModel : PageViewModel<DeliveryAssuranceListPage>
    {
        public DeliveryAssuranceListViewModel(DeliveryAssuranceListPage currentPage) : base(currentPage)
        {
            ListOfNotConfirmed = new List<DeliveryAssuranceListItem>();
            ListOfConfirmed = new List<DeliveryAssuranceListItem>();
            ListOfDelivered = new List<DeliveryAssuranceListItem>();
        }

        public List<DeliveryAssuranceListItem> ListOfNotConfirmed { get; set; }
        public List<DeliveryAssuranceListItem> ListOfConfirmed { get; set; }
        public List<DeliveryAssuranceListItem> ListOfDelivered { get; set; }

        public int NotConfirmedCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int DeliveredCount { get; set; }
        public bool IsInternal { get; set; }
        public bool IsShowCreateDelAssFromOtherLink { get; set; }
        public bool IsShowCreateNewLink { get; set; }
        public string CreateNewUrl { get; set; }
        public string BookDeliveryUrl { get; set; }
    }

    public class DeliveryAssuranceListItem
    {
        public DeliveryAssuranceListItem()
        {
            DeliveryAssurance = new DeliveryAssuranceList();
        }
        public DeliveryAssuranceListItem(DeliveryAssuranceList deliveryAssurance)
        {
            DeliveryAssurance = deliveryAssurance;
        }

        public DeliveryAssuranceListItem(DeliveryAssuranceList deliveryAssurance, string url, string cunr)
        {
            DeliveryAssurance = deliveryAssurance;
            
            ApproveDeliveryAssuranceUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{url}Approve", new Dictionary<string, string>()
            {
                {"a", DeliveryAssurance.IONumber},
                {"l", DeliveryAssurance.LineNumber.ToString()},
                {"cunr", cunr}
            });
            ChangeDeliveryAssuranceUrl = DeliveryAssurance.Status <= 35 ? DeliveryAssuranceHelper.BuildQueryUrl($"{url}Change", new Dictionary<string, string>()
            {
                {"a", DeliveryAssurance.IONumber},
                {"l", DeliveryAssurance.LineNumber.ToString()},
                {"cunr", cunr}
            }) : string.Empty;
            CreateDeliveryAssuranceFromExistingUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{url}Create", new Dictionary<string, string>()
            {
                {"a", DeliveryAssurance.IONumber},
                {"l", DeliveryAssurance.LineNumber.ToString()},
                {"cunr", cunr}
            });
            OverviewDeliveryAssuranceUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{url}Overview", new Dictionary<string, string>()
            {
                {"a", DeliveryAssurance.IONumber},
                {"l", DeliveryAssurance.LineNumber.ToString()}
            });
            Gardshamtning = DeliveryAssurance.Gardshamtning ? "Ja" : "Nej";
        }

        public DeliveryAssuranceList DeliveryAssurance { get; set; }
        public string Gardshamtning { get; set; }
        public string ApproveDeliveryAssuranceUrl { get; set; }
        public string ChangeDeliveryAssuranceUrl { get; set; }
        public string CreateDeliveryAssuranceFromExistingUrl { get; set; }
        public string OverviewDeliveryAssuranceUrl { get; set; }
    }
}