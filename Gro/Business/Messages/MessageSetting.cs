using EPiServer.Core;
using Gro.Core.ContentTypes.Utils;
using GroContentType = Gro.Core.ContentTypes.Pages;

namespace Gro.Business.Messages
{
    public static class MessageSetting
    {
        private static GroContentType.SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        public static ContentReference GetAdministerPage()
            => SettingPage?.AdminSettingMessage;

        public static ContentReference GetUserSettingPage()
            => SettingPage?.UserSettingMessage;

        public static ContentReference GetUserViewPage()
            => SettingPage?.UserViewMessage;

        public static int GetPageSizeForAdminPage() => SettingPage != null && SettingPage.PageSizeInAdminMessage > 1
            ? SettingPage.PageSizeInAdminMessage : 1;

        public static int GetPageSizeForUserPage()
            => SettingPage != null && SettingPage.PageSizeInViewMessage > 1 ? SettingPage.PageSizeInViewMessage : 1;

        public static string GetMessageForEmptyCategory()
            => SettingPage?.EmptyCategoryMessage;

        public static string GetMessageSignature()
            => SettingPage?.MessageSignature != null ? SettingPage.MessageSignature.ToHtmlString() : string.Empty;
    }
}
