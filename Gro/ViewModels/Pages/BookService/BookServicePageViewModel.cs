using Gro.Core.ContentTypes.Pages.BookService;
using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Pages.BookService
{
    public class BookServicePilotenViewModel : PageViewModel<BookServicePilotenPage>
    {
        public BookServicePilotenViewModel(BookServicePilotenPage currentPage) : base(currentPage)
        {
        }

        public CustomerBasicInfo Customer { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserEmail { get; set; }

        public string MachineModel { get; set; }

        public string MachineSerialNumber { get; set; }

        public string MachineRegister { get; set; }

        public string OwnerEmail { get; set; }

        public string City { get; set; }
    }
}
