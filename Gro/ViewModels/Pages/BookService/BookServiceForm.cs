using System.ComponentModel.DataAnnotations;

namespace Gro.ViewModels.Pages.BookService
{
    public class BookServiceForm
    {
        [Required(ErrorMessage = "Du måste ange Märke och modell")]
        public string MachineModel { get; set; }

        public string MachineSerialNumber { get; set; }

        public string MachineRegister { get; set; }

        [Required(ErrorMessage = "Meddelande är obligatorisk")]
        public string Message { get; set; }

        public string OwnerEmail { get; set; }

        [Required]
        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string BackLinkReference { get; set; }
        
        public string Reference { get; set; }
    }
}
