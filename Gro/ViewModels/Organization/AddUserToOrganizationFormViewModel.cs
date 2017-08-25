using System.ComponentModel.DataAnnotations;

namespace Gro.ViewModels.Organization
{
    public class AddUserToOrganizationFormViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Phone]
        public string Telephone { get; set; }

        [Required]
        [Phone]
        public string Mobile { get; set; }

        /// <summary>
        /// Comma-separated role ids
        /// </summary>
        [Required]
        public string Roles { get; set; }
    }
}
