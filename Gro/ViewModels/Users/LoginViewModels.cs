using System.ComponentModel.DataAnnotations;

namespace Gro.ViewModels.Users
{
    public class LostPasswordSubmissionViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }
    }
}
