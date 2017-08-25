using System.ComponentModel.DataAnnotations;

namespace Gro.ViewModels.Users
{
    public class EditProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Phone]
        public string Telephone { get; set; }

        [Required]
        [Phone]
        public string Mobilephone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Street { get; set; }

        [RegularExpression(@"^\d{5}$")]
        public string Zip { get; set; }
        public string City { get; set; }
    }
}
