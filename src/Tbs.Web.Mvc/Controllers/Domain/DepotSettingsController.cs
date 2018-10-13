using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Features;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tbs.Controllers;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class DepotSettingsController : TbsControllerBase
    {
        private readonly IRepository<DepotSetting> _repository;
        public DepotSettingsController(IRepository<DepotSetting> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

	}
}