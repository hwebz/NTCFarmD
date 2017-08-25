using EPiServer.Shell;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.Messages;

namespace Gro.Core.ContentTypes.Business.UIDescriptors
{
    [UIDescriptorRegistration]
    public class SettingsPageDescriptor : UIDescriptor<SettingsPage>
    {
        public SettingsPageDescriptor()
            : base(ContentTypeCssClassNames.Unknown)
        {
            this.DefaultView = CmsViewNames.AllPropertiesView;
            this.IconClass = ContentTypeCssClassNames.Unknown;
            this.DisabledViews = new[] { CmsViewNames.OnPageEditView, CmsViewNames.OnPageEditView };

        }
    }
}
