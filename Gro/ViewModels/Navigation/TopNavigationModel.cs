using System.Collections.Generic;
using EPiServer.Core;

namespace Gro.ViewModels.Navigation
{
    public class TopNavigationModel
    {
        public TopNavigationModel()
        {
            ListServiceStartPageItems = new List<NavigationItemModel>();
        }
        public List<NavigationItemModel> ListServiceStartPageItems { get; set; }
        public string StartPageText { get; set; }

        public ContentReference StartpageReference { get; set; }
    }
}
