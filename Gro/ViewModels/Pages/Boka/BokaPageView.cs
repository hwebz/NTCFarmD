using Gro.Core.ContentTypes.Pages.BokaPages;
using Gro.Core.DataModels.Boka;
using System.Collections.Generic;

namespace Gro.ViewModels.Pages.Boka
{
    public class BokaPageView : PageViewModel<BokaPage>
    {
        public BokaPageView(BokaPage currentPage) : base(currentPage)
        {
            ResourceItemList = new List<ResourceItemDto>();
            SearchTypes = new List<BokaSearchType>()
            {
                new BokaSearchType("5","Inköpsordernr"),
                new BokaSearchType("6","Kundordernr"),
                new BokaSearchType("7","Distributionsordernummer"),
                new BokaSearchType("8","Sändningsnummer"),
                new BokaSearchType("9","Kundnummer")
            };
            ListCompany = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("1", "Lantbruk"),
                new KeyValuePair<string, string>("2", "Agroetanol"),
                new KeyValuePair<string, string>("3", "Cerealia"),
            };
            SeachValue = string.Empty;
        }
        public List<ResourceItemDto> ResourceItemList { get; set; }

        public List<BokaSearchType> SearchTypes { get; set; }
        public CustomerInfo Customer { get; set; }
        public List<KeyValuePair<string, string>> ListCompany { get; set; }
        public string SeachValue { get; set; }
    }

    public class BokaSearchType
    {
        public BokaSearchType(string value, string name)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CustomerInfo
    {
        public CustomerInfo(string number, string name, string phone, string email)
        {
            CustomerNo = number;
            CustomerName = name;
            PhoneNumber = phone;
            OwnerEmail = email;
        }

        public CustomerInfo()
        {
            CustomerNo = string.Empty;
            CustomerName = string.Empty;
            PhoneNumber = string.Empty;
            OwnerEmail = string.Empty;
        }

        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerEmail { get; set; }
    }
}
