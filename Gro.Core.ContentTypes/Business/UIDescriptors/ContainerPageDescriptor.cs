using EPiServer.Shell;
using Gro.Core.ContentTypes.Pages;

namespace Gro.Core.ContentTypes.Business.UIDescriptors
{
    [UIDescriptorRegistration]
    public class ContainerPageDescriptor : UIDescriptor<FolderPage>
    {
        public ContainerPageDescriptor() : base(ContentTypeCssClassNames.Folder)
        {
            DefaultView = CmsViewNames.AllPropertiesView;
        }
    }
}
