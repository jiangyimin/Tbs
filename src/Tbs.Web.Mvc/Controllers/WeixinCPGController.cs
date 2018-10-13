using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.OAuth2;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Helpers;
using Tbs.Controllers;
using Tbs.Web.Models;
using Tbs.DomainModels;
using Tbs.Configuration;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class WeixinCPGController : TbsControllerBase
    {
        // private readonly string _corpId = "wx9b674b6377fc64be";
        // private readonly string _secret = "F0drNfu4Qdy6HStvuX6HsZMmcGcj7vc_R6tDvpWaLLw";

        private IWeixinAppService _weixinAppService;
        public WeixinCPGController(IWeixinAppService weixinAppService)
        {
            _weixinAppService = weixinAppService;
        }

        public ActionResult Index(string id, string code)
        {
            return View();
        }
        
        public ActionResult Grids(string id)
        {
            int tenantId = DomainManager.GetTenantIdByName(id);
            if (tenantId == 0) {
                return Content("没有对应的公司编号");
            }
            string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, tenantId).Result;
            string secret = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Secret, tenantId).Result;

            var jsapiticket = JsApiTicketContainer.GetTicket(corpId, secret);
            ViewBag.appId = corpId;
            ViewBag.noncestr = JSSDKHelper.GetNoncestr();
            ViewBag.timestamp = JSSDKHelper.GetTimestamp();
            ViewBag.signature = JSSDKHelper.GetSignature(jsapiticket, ViewBag.nonceStr, ViewBag.timestamp, AbsoluteUri());
            ViewBag.tenantId = tenantId;
            return View();
        }

        public ActionResult QrCode()
        {
            return View();
        }

        public ActionResult ReceiveBox(int id)
        {
            var dto = new WxOutletDto();

            using (CurrentUnitOfWork.SetTenantId(id))
            using (AbpSession.Use(id, null))
            {
                Worker worker = DomainManager.GetWorkerByCn("62536");
                dto.SetWorker(worker);
                dto.SetWorker2(DomainManager.GetWorkerByCn("62563"));
                dto.SetVehice(DomainManager.GetVehicles(worker.DepotId)[0]);

                dto.TaskBoxs.Add(new WeixinTaskBoxDto("07", "7号柜员箱"));
                dto.TaskBoxs.Add(new WeixinTaskBoxDto("12", "12号柜员箱"));
                dto.TaskBoxs.Add(new WeixinTaskBoxDto("Q1", "1号清分箱"));
            }
            return View(dto);
        }
    }
}