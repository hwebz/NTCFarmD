using Gro.Core.ContentTypes.Pages.MachinePages;
using Gro.Core.DataModels.Machine;

namespace Gro.ViewModels.Pages.Machines
{
    public class MachineDetailPageViewModel : PageViewModel<MachineDetailPage>
    {
        public MachineDetailPageViewModel(MachineDetailPage currentPage) : base(currentPage)
        {
            Machine= new Machine();
        }

        public bool IsHasMachineWRight { get; set; }

        public bool IsHasMachineOwnerPicture { get; set; }

        public Machine Machine { get; set; }

        private string _urlBookService;
        public string UrlBookService
        {
            get
            {
                return _urlBookService + "?model={0}&serial={1}&register={2}&reference=machine";
            }
            set
            {
                _urlBookService = value;
            }
        }
    }
}
