using Tbs.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace Tbs.Web.Host.Controllers
{
    public class AntiForgeryController : TbsControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}