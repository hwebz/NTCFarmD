using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MyProfile;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Gro.Business.Initialization
{
    /// <summary>
    /// Module for customizing templates and rendering.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
    public class ContentEventsHandlers : IInitializableModule
    {
        private static IContentEvents ContentEvents => ServiceLocator.Current.GetInstance<IContentEvents>();

        public void Initialize(InitializationEngine context)
        {
            ContentEvents.PublishingContent += ContentPublishing;
            ContentEvents.PublishedContent += ContentPublished;
        }

        public void Uninitialize(InitializationEngine context)
        {
            ContentEvents.PublishingContent -= ContentPublishing;
        }

        private static void ContentPublished(object sender, ContentEventArgs e)
        {
            if (!(e.Content is UserAgreementsPage)) return;

            var userAgreementPage = (UserAgreementsPage) e.Content;
            // handler when there is new version of user agreements.
            // call to update new version of user agreements.
            var userManagerService = ServiceLocator.Current.GetInstance<IUserManagementService>();
            userManagerService.UpdateTermsOfUseVersion(userAgreementPage.Version, userAgreementPage.TermId);
        }

        private static void ContentPublishing(object sender, ContentEventArgs e)
        {
            if (!(e.Content is UserAgreementsPage)) return;

            var userAgreements = (UserAgreementsPage)e.Content;
            var clone = userAgreements.CreateWritableClone() as UserAgreementsPage;
            if (clone == null) return;

            clone.Version = ++clone.Version;
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            contentRepository.Save(clone, SaveAction.ForceCurrentVersion, AccessLevel.NoAccess);
        }
    }
}
