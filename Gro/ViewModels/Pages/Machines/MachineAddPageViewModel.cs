using Gro.Core.ContentTypes.Pages.MachinePages;
using Gro.Core.DataModels.Machine;
using System.Collections.Generic;

namespace Gro.ViewModels.Pages.Machines
{
    public class MachineAddPageViewModel : PageViewModel<MachineAddPage>
    {
        public MachineAddPageViewModel(MachineAddPage currentPage) : base(currentPage)
        {
            CategoryList = new List<MachineCategory>();
            BrandList = new List<MachineBrand>();
        }

        public List<MachineCategory> CategoryList { get; set; }

        public List<MachineBrand> BrandList { get; set; }

        public string UrlMaskinStarPage { get; set; }
    }

    public class MachineAddModelList
    {
        public IEnumerable<MachineModel> ListModels { get; set; }
        public  MachineModel SelectedModel { get; set; }
    }
}