using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(DisplayName = "ExternalArticlePage", GUID = "010A51E4-D670-430F-A08D-08E0C535E893", Description = "")]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(ExternalArticlePage)})]
    public class ExternalArticlePage : ArticlePage
    {
    }
}