using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Users
{
    public class CustomerListViewModel
    {
        public CustomerListViewModel(CustomerBasicInfo activeCustomer, CustomerBasicInfo[] customerList)
        {
            ActiveCustomer = activeCustomer;
            CustomerList = customerList;
        }
        //public Dictionary<string, string> CustomerList { get; set; }
        //public string ActiveCustomerNumber { get; set; }

        //public CustomerBasicInfo ActiveCustomer { get; set; }
        //public CustomerBasicInfo[] CustomerList { get; set; }
        public CustomerBasicInfo ActiveCustomer { get; set; }
        public CustomerBasicInfo[] CustomerList { get; set; }
    }
}
