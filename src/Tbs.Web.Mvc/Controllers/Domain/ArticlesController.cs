using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Authorization;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Data, PermissionNames.KeeperPages)]
    public class ArticlesController : TbsCrudController<Article, ArticleDto>
    {
        public ArticlesController(IRepository<Article> repository)
            : base(repository)
        {
        }
	}
}