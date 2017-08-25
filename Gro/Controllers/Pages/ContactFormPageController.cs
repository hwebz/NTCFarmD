using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.Interfaces;
using Gro.ViewModels;

namespace Gro.Controllers.Pages
{
    [TemplateDescriptor(Inherited = true)]
    public class ContactFormPageController : SiteControllerBase<ContactFormPage>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public ContactFormPageController(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public ViewResult Index(ContactFormPage currentPage)
        {
            return View("Index", new PageViewModel<ContactFormPage>(currentPage));
        }

        [HttpPost]
        public async Task<ViewResult> Index(ContactFormPage currentPage, string subject, string message, string sendAlso,
            string name = "", string email = "", string customerNumber = "")
        {
            var emailForInsert = email;
            var nameForInsert = name;
            var customerNumberForInsert = customerNumber;
            if (SiteUser != null)
            {
                nameForInsert = SiteUser.Name;
                emailForInsert = SiteUser.Email;
            }
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            if (activeCustomer != null)
            {
                customerNumberForInsert = activeCustomer.CustomerNo;
            }

            var isSentSucceess = await _organizationRepository.ContactCustomerServiceAsync(subject, message, sendAlso != null,
                emailForInsert, nameForInsert, customerNumberForInsert);
            ViewBag.hasSentMessage = isSentSucceess;
            return View("Index", new PageViewModel<ContactFormPage>(currentPage));
        }
    }
}
