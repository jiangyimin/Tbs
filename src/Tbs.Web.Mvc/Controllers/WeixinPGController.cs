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
    public class WeixinPGController : TbsControllerBase
    {
        // private readonly string _corpId = "wx9b674b6377fc64be";
        // private readonly string _secret = "F0drNfu4Qdy6HStvuX6HsZMmcGcj7vc_R6tDvpWaLLw";

        private IWeixinAppService _weixinAppService;
        public WeixinPGController(IWeixinAppService weixinAppService)
        {
            _weixinAppService = weixinAppService;
        }

        public ActionResult Signin(string id, string code)
        {
            int tenantId = DomainManager.GetTenantIdByName(id);
            if (tenantId == 0) {
                return Content("没有对应的公司编号");
            }
            string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, tenantId).Result;
            string secret = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Secret, tenantId).Result;

            if (string.IsNullOrEmpty(code))
            {
                return Redirect(OAuth2Api.GetCode(corpId, AbsoluteUri(), "STATE"));
            }

            var accessToken = AccessTokenContainer.GetToken(corpId, secret);
            GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);

            var jsapiticket = JsApiTicketContainer.GetTicket(corpId, secret);
            ViewBag.appId = corpId;
            ViewBag.noncestr = JSSDKHelper.GetNoncestr();
            ViewBag.timestamp = JSSDKHelper.GetTimestamp();
            ViewBag.signature = JSSDKHelper.GetSignature(jsapiticket, ViewBag.nonceStr, ViewBag.timestamp, AbsoluteUri());
            ViewBag.tenantId = tenantId;
            ViewBag.workerCn = userInfo.UserId;

            Logger.Warn($"{jsapiticket} {ViewBag.noncestr} {ViewBag.timestamp} {ViewBag.signature} {AbsoluteUri()}");
            return View();
        }

        [HttpPost]
        public ActionResult DoSignin(int tenantId, string workerCn, double? lon, double? lat, double? accuracy)
        {
            ViewBag.message = _weixinAppService.Signin(tenantId, workerCn, lon.Value, lat.Value, accuracy.Value);
            return View("notify");
        }

        public ActionResult Identify(string id, string code)
        {
            int tenantId = DomainManager.GetTenantIdByName(id);
            if (tenantId == 0) {
                return Content("没有对应的公司编号");
            }
            string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, tenantId).Result;
            string secret = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Secret, tenantId).Result;

            if (string.IsNullOrEmpty(code))
            {
                return Redirect(OAuth2Api.GetCode(corpId, AbsoluteUri(), "STATE"));
            }

            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            
            if (dto == null)
            {
                var accessToken = AccessTokenContainer.GetToken(corpId, secret);
                GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                WxLoginViewModel vm = new WxLoginViewModel() 
                {
                    WorkerCn = userInfo.UserId,
                    DeviceId = userInfo.DeviceId,
                    TenantId = tenantId,
                    ReturnUrl = AbsoluteUri()
                };
                
                return View("login", vm);
            }
            else
            {
                if (dto.UseRoute)
                     return View("TaskList", dto);
                else
                    return View(dto);
            }
        }

        [HttpPost]
        public ActionResult DoIdentify()
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            // clear something
            dto.TaskId = 0;
            dto.OutletCn = dto.OutletName = null;
            HttpContext.Session.SetObjectAsJson("WxIdentify", dto);                
            
            return View("Identify", dto);
        }

        [HttpPost]
        public ActionResult SelectTask(int taskId, string outletCn)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            dto.TaskId = taskId;
            SelectOutlet(outletCn, dto);
            return View("Identify", dto);
        }

        [HttpPost]
        public ActionResult SelectOutlet(string outletCn)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            if (string.IsNullOrWhiteSpace(outletCn))
            {
                ModelState.AddModelError("", "需要输入网点编号");
                return View("Identify", dto);
            }

            if (SelectOutlet(outletCn, dto))
            {
                return View("Identify", dto);
            }
            else
            {
                ModelState.AddModelError("", "此编号没有对应的网点");
                return View("Identify", dto);
            }
        }

        [HttpPost]
        public ActionResult VerifyOutlet(int taskId, string outletCn, string password)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            if (dto != null && !string.IsNullOrEmpty(password) && dto.OutletPassword == password)
            {
                if (dto.UseRoute)
                {
                    var outlet = DomainManager.GetOutlets(dto.DepotId).FirstOrDefault(o => o.Cn == dto.OutletCn);
                    _weixinAppService.SetIdentifyTime(taskId, dto.RouteId, outlet != null ? outlet.Id : 0);
                }
                return View("Information2", dto);
            }
            else
            {
                ModelState.AddModelError("", "网点密码不符，请重新输入");
                return View("Identify", dto);
            }
        }

        [HttpPost]
        public ActionResult Login(WxLoginViewModel vm)
        {
            WxIdentifyDto dto = _weixinAppService.Login(vm.TenantId, vm.WorkerCn, vm.Password, vm.DeviceId);
            if (!string.IsNullOrEmpty(dto.ErrorMessage))
            {
                ModelState.AddModelError("", dto.ErrorMessage);
                return View(vm);
            }

            HttpContext.Session.SetObjectAsJson("WxIdentify", dto);
            return Redirect(vm.ReturnUrl);
        }

        #region utils

        private bool SelectOutlet(string outletCn, WxIdentifyDto dto)
        {
            // Logger.Warn($"选择网点时：{dto.TenantId} {dto.DepotId} {outletCn}");
            using (CurrentUnitOfWork.SetTenantId(dto.TenantId))
            using (AbpSession.Use(dto.TenantId, null))
            {
                Outlet outlet = DomainManager.GetOutlets(dto.DepotId).FirstOrDefault(o => o.Cn == outletCn);
                // Logger.Warn($"{outlet}");
                if (outlet != null)
                {
                    dto.OutletCn = outlet.Cn;
                    dto.OutletName = outlet.Name;
                    dto.OutletPassword = outlet.Password;
                    dto.OutletCipertext = outlet.Ciphertext;
                    HttpContext.Session.SetObjectAsJson("WxIdentify", dto);                
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}