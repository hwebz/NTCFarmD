using Gro.Core.ContentTypes.Business;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Gro.ViewModels.Registration
{
    public abstract class ActivationBaseForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string CustomerNumber { get; set; }

        [Required]
        public string OrganizationNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public string Action { get; set; }

        [RegularExpression(@"^(\+46)\d{6,}$")]
        public string Telephone { get; set; }

        [Required]
        public string Mobilephone { get; set; }
    }

    public class PrivateFirmActivationForm : ActivationBaseForm
    {
        public static PrivateFirmActivationForm FromRequest(HttpRequestBase request) => new PrivateFirmActivationForm
        {
            Action = request.Form[nameof(Action)],
            CustomerNumber = request.Form[nameof(CustomerNumber)],
            OrganizationNumber = request.Form[nameof(OrganizationNumber)],
            FirstName = request.Form[nameof(FirstName)],
            LastName = request.Form[nameof(LastName)],
            Email = request.Form[nameof(Email)],
            SerialNumber = request.Form[nameof(SerialNumber)],
            Mobilephone = request.Form[nameof(Mobilephone)],
            Telephone = request.Form[nameof(Telephone)]
        };
    }

    public class NonPrivateFirmActivationForm : ActivationBaseForm
    {
        [Required]
        public string CustomerName { get; set; }

        public static NonPrivateFirmActivationForm FromRequest(HttpRequestBase request) => new NonPrivateFirmActivationForm
        {
            Action = request.Form[nameof(Action)],
            CustomerNumber = request.Form[nameof(CustomerNumber)],
            OrganizationNumber = request.Form[nameof(OrganizationNumber)],
            FirstName = request.Form[nameof(FirstName)],
            LastName = request.Form[nameof(LastName)],
            Email = request.Form[nameof(Email)],
            SerialNumber = request.Form[nameof(SerialNumber)],
            Mobilephone = request.Form[nameof(Mobilephone)],
            Telephone = request.Form[nameof(Telephone)],
            CustomerName = request.Form[nameof(CustomerName)],
        };
    }
}
