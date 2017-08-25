using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.DataModels.DeliveryAssuranceDtos;

namespace Gro.ViewModels.Pages.AppPages.DeliveryAssurance
{
    public class DeliveryAssuranceOverviewViewModel : PageViewModel<DeliveryAssuranceListPage>
    {
        public DeliveryAssuranceOverviewViewModel(DeliveryAssuranceListPage currentPage) : base(currentPage)
        {
            DeliveryAssurance = new DeliveryAssuranceOverview();
        }

        public DeliveryAssuranceOverview DeliveryAssurance { get; set; }
        public string ChangeUrl { get; set; }

        public bool IsMultiApprove { get; set; }
    }

    public class DeliveryAssuranceOverview
    {
        public DeliveryAssuranceOverview()
        {
            Address = new DeliveryAddress();
        }
        public string LorryTypeDesc { get; set; }
        public string DeliveryAddress { get; set; }
        public string ItemName { get; set; }
        public string WarehouseDesc { get; set; }
        public string DepaAvtal { get; set; }
        public string SlamDesc { get; set; }
        public string StraforkortatDesc { get; set; }
        public string TorkatDesc { get; set; }
        public string RedDesc { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string IONumber { get; set; }
        public bool Gardshamtning { get; set; }
        public string TermAndCondition { get; set; }
        public string DeliveryDate { get; set; }
        public double Quantity { get; set; }
        public string HarvestYear { get; set; }
        public string OtherInfo { get; set; }
        public string TransportType { get; set; }
        public string DeliveryTypeDesc { get; set; }
        public string Action { get; set; }
        public int Status { get; set; }
        public string KundorderNr { get; set; }
        public string KundsOrderNr { get; set; }
        public string GHGvarde { get; set; }

        public DeliveryAddress Address { get; set; }
    }
}