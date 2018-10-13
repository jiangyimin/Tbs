using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Tbs.Web.Views
{
    public abstract class TbsRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TbsRazorPage()
        {
            LocalizationSourceName = TbsConsts.LocalizationSourceName;
        }
    }
}
