using EPiServer.Web.Mvc;
using Gro.Business.DataProtection;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.Interfaces;
using Gro.ViewModels;
using Gro.ViewModels.Ekonomi;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gro.Controllers.Pages
{
    public partial class EkonomiIframeStartPageController : PageController<EkonomiIframeStartPage>
    {
        private readonly IUserManagementService _userManager;
        private readonly HttpContextBase _httpContext;
        private JwtTokenGenerator _tokenGenerator;
        private readonly IOrganizationUserRepository _organizationUserRepository;

        public EkonomiIframeStartPageController(IUserManagementService userManager, IOrganizationUserRepository organizationUserRepository, HttpContextBase httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
            _tokenGenerator = new JwtTokenGenerator(ConfigurationManager.AppSettings["EkonomiSsoKey"]);
            _organizationUserRepository = organizationUserRepository;
        }

        public ActionResult Index(EkonomiIframeStartPage currentPage)
        {
            var user = _userManager.GetSiteUser(_httpContext);
            var roles = _organizationUserRepository.GetUserCustomerRoles(user.UserName, user.ActiveCustomerNumber);
            
            ViewData["payload"] = _tokenGenerator.Encrypt(new EkonomiUserModel {
                CustomerNumber = user.ActiveCustomerNumber,
                Username = user.Email,
                UtcTime = DateTime.UtcNow,
                Rights = roles.Select(x => new EkonomiUserRightsModel { Id = x.Roleid, Name = x.RoleName })
                    .Where(x => x.Id == 10 || x.Id == 3).ToList()
            });

            return View("Index", new PageViewModel<EkonomiIframeStartPage>(currentPage));
        }
    }
}