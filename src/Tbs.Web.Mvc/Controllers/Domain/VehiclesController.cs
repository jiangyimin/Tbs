using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using System.Threading.Tasks;
using Abp.Extensions;
using Tbs.Features;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Data)]
    public class VehiclesController : TbsCrudController<Vehicle, VehicleDto>
    {
        public VehiclesController(IRepository<Vehicle> repository)
            :base(repository)
        {
        }

        public override async Task<JsonResult> CreateEntity(VehicleDto input)
        {
            var count = await Repository.CountAsync();
            string maxVehicles = await FeatureChecker.GetValueAsync(FeatureNames.MaxVehicles);
            if (count >= maxVehicles.To<int>())
                throw new UserFriendlyException("功能限制", "超过最大个数");

            return await base.CreateEntity(input);
        }

        public ActionResult GetPhoto(int id)
        {
            Vehicle v = Get(id);
            if (v != null) {
                if (v.Photo != null)
                    return File(v.Photo, "image/jpg");
                else
                    return File(new byte[0], "image/jpg");
            }
            else
                throw new UserFriendlyException("无此车辆");

        }
	}
}