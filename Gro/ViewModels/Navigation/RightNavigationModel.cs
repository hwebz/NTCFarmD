using System.Collections.Generic;
namespace Gro.ViewModels.Navigation
{
    public class RightNavigationModel
    {
        public RightNavigationModel(bool isNavWithHeader = false)
        {
            ListNavigationItems = new List<NavigationItemModel>();
            IsNavWithHeader = isNavWithHeader;
        }
        public bool IsNavWithHeader { get; set; }
        public List<NavigationItemModel> ListNavigationItems { get; set; }
        public string Header { get; set; }
    }
}
