using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Features;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Tbs.Editions;
using Abp.Extensions;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class DepotsController : TbsCrudController<Depot, DepotDto>
    {
        public DepotsController(IRepository<Depot> repository)
            :base(repository)
        {
        }

        public override async Task<JsonResult> CreateEntity(DepotDto input)
        {
            var count = await Repository.CountAsync();
            string maxDepots = await FeatureChecker.GetValueAsync(FeatureNames.MaxDepots);
            if (count >= maxDepots.To<int>())
                throw new UserFriendlyException("功能限制", "超过最大个数");

            return await base.CreateEntity(input);
        }
	}
}