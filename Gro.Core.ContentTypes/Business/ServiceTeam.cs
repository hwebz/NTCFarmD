using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Business
{
    public enum ServiceTeam
    {
        [Display(Name = "Nötfor")]
        NotFor,

        [Display(Name = "Piggfor")]
        Piggfor,

        [Display(Name = "Pullfor")]
        Pullfor,

        [Display(Name = "Växtodling")]
        Vaxtodling,

        [Display(Name = "Maskin")]
        Maskin
    }
}
