using System.Web;
using System.Web.Mvc;

namespace Gro.Helpers
{
    public static class StringHelper
    {
        public static IHtmlString ToLineBreakString(this string original)
        {
            var parsed = string.Empty;
            if (string.IsNullOrEmpty(original)) return new MvcHtmlString(parsed);
            parsed = HttpUtility.HtmlEncode(original);
            parsed = parsed.Replace("\n", "<br />");

            return new MvcHtmlString(parsed);
        }

        public static string ReplaceBrToReturn(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Replace(@"<br/>", "\r\n");
        }

        public static string ReplaceReturnToBr(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Replace("\r\n", @"<br/>");
        }

    }
}
