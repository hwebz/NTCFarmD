using Gro.Business;
using Gro.Business.DataProtection;
using Gro.Constants;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Gro.Helpers
{
    public static class SiteHttpExtensions
    {
        private static ITokenGenerator TokenGenerator => DependencyResolver.Current.GetService<ITokenGenerator>();

        public static string GetSerialNumber(this HttpContextBase httpContext)
        {
            var serialNumber = httpContext.Request.Headers.Get("serialnumber");
            return string.IsNullOrWhiteSpace(serialNumber) || serialNumber == "NOT_FOUND" ? null : serialNumber;
        }

        /// <summary>
        /// Get the cookie site user
        /// </summary>
        public static SiteUser GetSiteUser(this HttpContextBase httpContext)
        {
            var siteUserCookie = httpContext.Request.Cookies?.Get(Cookies.SiteUser)?.Value;
            if (string.IsNullOrWhiteSpace(siteUserCookie)) return null;

            try
            {
                var siteUser = TokenGenerator.Decrypt<SiteUser>(siteUserCookie);
                siteUser.SerialNumber = httpContext.GetSerialNumber();
                return siteUser;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is CryptographicException)
            {
                return null;
            }
        }

        /// <summary>
        /// Set a cookie
        /// </summary>
        /// <returns>Set cookie result</returns>
        public static bool SetCookie(this HttpContextBase httpContext, string name, string value, bool httpOnly = false)
        {
            if (httpContext?.Response?.Cookies == null) return false;

            httpContext.Response.Cookies.Set(new HttpCookie(name, value));
            return true;
        }

        /// <summary>
        /// Invalidate a cookie
        /// </summary>
        public static void InvalidateCookie(this Controller controller, string cookieName)
        {
            if (controller?.Response?.Cookies?[cookieName] == null) return;
            //invalidate

            var newCookie = new HttpCookie(cookieName, null);
            controller.Response.Cookies.Set(newCookie);
            controller.Request.Cookies.Set(newCookie);

            //var cookie = controller.Response.Cookies[cookieName];
            //cookie.Expires = DateTime.Now.AddDays(-1);
            //controller.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Sign an user in
        /// </summary>
        public static void SetUserSession(this Controller controller, SiteUser siteUser)
        {
            //clear the user's previous session
            controller.SignoutSiteUser();

            var userCookieToken = TokenGenerator.Encrypt(siteUser);
            controller.HttpContext.SetCookie(Cookies.SiteUser, userCookieToken, true);
        }

        /// <summary>
        /// Sign an user out
        /// </summary>
        public static void SignoutSiteUser(this Controller controller)
        {
            if (controller?.Response?.Cookies?[Cookies.SiteUser] == null) return;

            //invalidate
            controller.InvalidateCookie(Cookies.SiteUser);
            controller.InvalidateCookie(Cookies.ActiveCustomer);
        }

        //to override EPiserver login page, consider Unauthorized a bad request
        private static HttpStatusCodeResult UnAuthorizedResult() => new HttpStatusCodeResult(400, "Unauthorized");

        private static void Write401(this ControllerContext actionContext)
        {
            actionContext.HttpContext.Response.StatusCode = 401;
            actionContext.HttpContext.Response.Write("Unauthorized");
            actionContext.HttpContext.Response.End();
        }

        public static void ReturnUnAuthorizedResult(this ActionExecutingContext actionContext)
        {
#if DEBUG
            actionContext.Write401();
#else
            actionContext.Result = UnAuthorizedResult();
#endif
        }

        public static void ReturnUnAuthorizedResult(this AuthorizationContext actionContext)
        {
#if DEBUG
            actionContext.Write401();
#else
            actionContext.Result = UnAuthorizedResult();
#endif
        }

        public static string GetLoginBankAccountUrl(this HttpRequestBase request, string returnUrl = "")
        {
            var configurationForUrl = ConfigurationManager.AppSettings["LoginBankAccountUrl"];
            if (string.IsNullOrEmpty(configurationForUrl)) return string.Empty;

            returnUrl = string.IsNullOrEmpty(returnUrl) ? $"{ConfigurationManager.AppSettings["domainUrl"]}/{request.Url?.PathAndQuery}" : returnUrl;
            returnUrl = System.Net.WebUtility.UrlEncode(returnUrl);
            var additionParams = $"&NameIdFormat=transient&Target={returnUrl}";
            return string.Format(configurationForUrl, additionParams);
        }
    }
}
