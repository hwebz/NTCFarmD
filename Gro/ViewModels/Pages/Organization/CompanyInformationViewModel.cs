using System.ComponentModel.DataAnnotations;
using Gro.Core.ContentTypes.Pages.Organization;

namespace Gro.ViewModels.Pages.Organization
{
    public class CompanyInformationViewModel : PageViewModel<CompanyInformationPage>
    {
        public CompanyInformationViewModel(CompanyInformationPage currentPage) : base(currentPage)
        {
        }

        public CompanyInformationModel CompanyInformation { get; set; }

        public bool IsLoginBankId { get; set; }
    }

    public class CompanyInformationModel
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "The email is not valid")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "The phone mobile is not valid")]
        public string PhoneMobile { get; set; }
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "The phone work is not valid")]
        public string PhoneWork { get; set; }
        public string Comment { get; set; }
    }
}