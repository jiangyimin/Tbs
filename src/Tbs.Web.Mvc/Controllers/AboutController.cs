using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tbs.Web.Controllers
{
    public class AboutController : TbsControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}