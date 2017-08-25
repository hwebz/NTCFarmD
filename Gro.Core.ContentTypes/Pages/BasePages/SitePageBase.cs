using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BlockAsProperties;
using Gro.Core.ContentTypes.Utils;
using System.Reflection;
using System.ComponentModel;

namespace Gro.Core.ContentTypes.Pages.BasePages
{
    public abstract class SitePageBase : PageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string Heading
        {
            get
            {
                var heading = this.GetPropertyValue(p => p.Heading);

                //if Heading not set fallbak to pagename
                return !string.IsNullOrWhiteSpace(heading) ? heading : Name;
            }
            set { this.SetPropertyValue(p => p.Heading, value); }
        }

        [Display(GroupName = GroupNames.SEO, Order = 500)]
        public virtual SeoBlock Seo { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            var properties = GetType().BaseType?.GetProperties() ?? new PropertyInfo[0];
            foreach (var property in properties)
            {
                var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttribute == null) continue;

                this[property.Name] = defaultValueAttribute.Value;
            }

            base.SetDefaultValues(contentType);
        }
    }
}
