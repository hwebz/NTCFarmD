using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.DataModels.Organization;

namespace Gro.ViewModels.Pages.Organization
{
    public class BusinessProfileViewModel : PageViewModel<BusinessProfilePage>
    {
        public BusinessProfileViewModel(BusinessProfilePage currentPage) : base(currentPage)
        {
        }

        public bool IsLoginBankId { get; set; }
        public BusinessProfileModel BusinessProfile { get; set; }
    }

    public class BusinessProfileModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<ProfileRow> Rows { get; set; }
    }
}
