using Gro.Core.ContentTypes.Pages.SearchTransport;
using Gro.Core.DataModels.SearchTransport;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using SearchCategories = Gro.Constants.SearchTransportCatogories;

namespace Gro.ViewModels.Pages.SearchTransport
{
    public class SearchTransportPageView : PageViewModel<SearchTransportPage>
    {
        public SearchTransportPageView(SearchTransportPage currentPage) : base(currentPage)
        {
            ListTransport = new List<ShipmentResponse>();
            ListOrder = new List<OrderRowResponse>();
            ListCategory = new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string, string>(SearchCategories.CustomerNumber, SearchCategories.CustomerNumber),
               new KeyValuePair<string, string>(SearchCategories.Ordernummer, SearchCategories.Ordernummer),
               new KeyValuePair<string, string>(SearchCategories.ShipmentId, SearchCategories.ShipmentId),
               new KeyValuePair<string, string>(SearchCategories.CustomerOrderNumber, SearchCategories.CustomerOrderNumber),
               new KeyValuePair<string, string>(SearchCategories.Carrier, SearchCategories.Carrier),
               new KeyValuePair<string, string>(SearchCategories.WayBill, SearchCategories.WayBill),
            };
            TotalBestKvant = 0;
            TotalLevKvant = 0;
            IsInternal = true;
        }

        public bool IsInternal { get; set; }
        public SearchTransportInfo SearchInfo { get; set; }
        public List<ShipmentResponse> ListTransport { get; set; }
        public List<OrderRowResponse> ListOrder { get; set; }
        //public List<SearchTransportCategory> ListCategory { get; set; }
        public List<KeyValuePair<string, string>> ListCategory { get; set; }
        public int TotalBestKvant { get; set; }
        public int TotalLevKvant { get; set; }
        public string UrlTrackSchenker
        {
            get
            {
                var url = ConfigurationManager.AppSettings["TrackSchenkerUrl"];
                var userId = ConfigurationManager.AppSettings["TrackSchenkerUserID"];
                var password = ConfigurationManager.AppSettings["TrackSchenkerPassword"];

                url = url.Replace("{userId}", userId);
                url = url.Replace("{password}", password);
                return url;
            }
        }
    }

    public class SearchTransportInfo
    {
        private string _searchText;
        public string Category { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string SearchText
        {
            get
            {
                var resultStr = !string.IsNullOrEmpty(_searchText) ? _searchText.Replace("/", string.Empty).Replace("\\", string.Empty) : string.Empty;
                return resultStr.Trim();
            }
            set
            {
                _searchText = value;
            }
        }
        public bool IsDisableDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(Category))
                {
                    return true;
                }
                return !Category.Equals(SearchCategories.CustomerNumber) && !Category.Equals(SearchCategories.Carrier);
            }
        }
    }

    public class SearchTransportCategory
    {
        public SearchTransportCategory(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
