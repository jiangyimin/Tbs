using Abp.AspNetCore.Mvc.Controllers;
using Abp.Application.Services.Dto;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using Tbs.DomainServices;
using Tbs.Authorization.Users;
using Abp.Runtime.Caching;

namespace Tbs.Controllers
{
    public abstract class TbsControllerBase: AbpController
    {
        public DomainManager DomainManager { get; set; }
        protected TbsControllerBase()
        {
            LocalizationSourceName = TbsConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected PagedAndSortedResultRequestDto GetPagedInput()
        {
            PagedAndSortedResultRequestDto input = new PagedAndSortedResultRequestDto();
            input.Sorting = GetSorting();
            input.MaxResultCount = int.Parse(Request.Form["rows"]);
            input.SkipCount = (int.Parse(Request.Form["page"]) - 1) * input.MaxResultCount;
            return input;
        }

        protected string GetSorting()
        {
            return $"{Request.Form["sort"]} {Request.Form["order"]}";
        }

        protected string AbsoluteUri()
        {
            return $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            // var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            // return location.AbsoluteUri;
        }

    }
}