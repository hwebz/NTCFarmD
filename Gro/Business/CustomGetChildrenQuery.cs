using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Business
{
    /// <summary>
    /// HIDE REGION PAGE CONTAINER IN THE PAGE TREE
    /// See more: https://tedgustaf.com/blog/2013/hide-pages-in-the-page-tree-in-episerver-7/
    /// </summary>
    /// <seealso cref="GetChildrenQuery" />
    [ServiceConfiguration(typeof(IContentQuery))]
    public class CustomGetChildrenQuery : GetChildrenQuery
    {

        public CustomGetChildrenQuery(IContentQueryHelper queryHelper, IContentRepository contentRepository,
            LanguageSelectorFactory languageSelectorFactory) : base(queryHelper, contentRepository, languageSelectorFactory)
        {
        }

        public override int Rank => 100;

        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
            => base.GetContent(parameters).Where(x => !x.ContentLink.Equals(ContentExtensions.GetSettingsPage().MachineMediaFolder));
    }
}
