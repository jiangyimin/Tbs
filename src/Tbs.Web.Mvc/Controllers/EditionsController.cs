using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Tbs.Authorization;
using Tbs.Controllers;
using Tbs.Editions;


namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.HostPages)]
    public class EditionsController : TbsControllerBase
    {
        private readonly IEditionAppService _editionAppService;
        public EditionsController(IEditionAppService editionAppService) 
        {
            _editionAppService = editionAppService;
        }

        public ActionResult Index()
        {
            var features = FeatureManager.GetAll();
            return View(features);
        }

        [DontWrapResult]
        public JsonResult GridData()
        {
            var editions = _editionAppService.GetEditions();
            return Json(new { rows = editions.Items }); 
        }

        [DontWrapResult]
        public JsonResult GetFeatures(int id)
        {
            var features = _editionAppService.GetFeatures(id);

            return Json(new {total=features.Count(), rows=features}); 
        }
	}
}