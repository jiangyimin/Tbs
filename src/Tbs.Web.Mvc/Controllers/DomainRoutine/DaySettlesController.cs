using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using System;
using Tbs.Web.Models;
using Abp.UI;
using Tbs.DomainModels;
using Tbs.Web.MessageHandlers;
using System.Text.RegularExpressions;
using Tbs.Configuration;
using Tbs.DomainServices;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DispatcherPages, PermissionNames.AdminPages)]
    public class DaySettlesController : TbsControllerBase
    {
        public readonly ISigninAppService _signinAppService;
        private readonly ISettleAppService _settleAppService;
        private readonly IRouteAppService _routeAppService;
        private readonly IVtAffairAppService _vtAffairAppService;
        private readonly IWhAffairAppService _whAffairAppService;
        private readonly IRecordAppService _recordAppService;

        public DaySettlesController(ISigninAppService signinAppService, ISettleAppService settleAppService, IRouteAppService routeAppService, IVtAffairAppService vtAffairAppSevice,
            IWhAffairAppService whAffairAppService, IRecordAppService recordService)
        {
            _signinAppService = signinAppService;
            _settleAppService = settleAppService;
            _routeAppService = routeAppService;
            _vtAffairAppService = vtAffairAppSevice;
            _whAffairAppService = whAffairAppService;
            _recordAppService = recordService;
        }

        public ActionResult Index()
        {
            var user = DomainManager.GetCurrentUser();
            var vm = new DaySettlesViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = user.GetOpPassword(),
                PwdDeadline = user.GetOpDeadline().ToString("yyyy-MM-dd HH"),
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        public ActionResult DaySettlesQuery()
        {
            var user = DomainManager.GetCurrentUser();
            var vm = new DaySettlesViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
        public async Task<JsonResult> GridPagedData(int id)
        {
            var output = await _settleAppService.GetPagedResult(id, GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult DaySettle(DateTime carryoutDate, string style)
        {
            try 
            {
        
                int depotId = DomainManager.GetDepotId();
                string agent = _signinAppService.GetAgentInfo(depotId, carryoutDate);
                int settleId = _settleAppService.CreateOrGet(depotId, carryoutDate);
                string replyMessage =  $"({DomainManager.GetDepotById(depotId).Name}){DateTime.Now.ToString("HH:mm")}用{style}{agent}进行了日结。情况如下：";
                replyMessage += "【" + _routeAppService.DaySettle(depotId, carryoutDate, settleId) + "】";
                replyMessage += "【" + _recordAppService.DaySettle(depotId, carryoutDate, settleId) + "】";
                replyMessage += "【" + _vtAffairAppService.DaySettle(depotId, carryoutDate, settleId) + "】";
                replyMessage += "【" + _whAffairAppService.DaySettle(depotId, carryoutDate, settleId) + "】";

                var user = DomainManager.GetCurrentUser();
                if (!string.IsNullOrEmpty(user.WhName)) // && Regex.IsMatch(user.WhName, @"^\d{5}$"))
                { 
                    string toUser = user.WhName;       
                    string managers = SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.ManagersForDaySettleNotify, AbpSession.TenantId.Value).Result;
                    if (!string.IsNullOrEmpty(managers))
                        toUser += "|" + managers;

                    WeixinUtils.SendMessage(AbpSession.TenantId.Value, toUser, replyMessage);
                }

                return Json( new {result="ok"});
            }
            catch (Exception ex)
            {
                throw new Abp.UI.UserFriendlyException("日结中发生错误；" + ex.Message);
            }
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult DayCheckRoutes(DateTime id)
        {
            string ret = _routeAppService.DaySettle(DomainManager.GetDepotId(), id, 0);
            return ReturnJsonResult(ret);
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult DayCheckVtAffairs(DateTime id)
        {
            string ret = _vtAffairAppService.DaySettle(DomainManager.GetDepotId(), id, 0);
            return ReturnJsonResult(ret);
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult DayCheckWhAffairs(DateTime id)
        {
            string ret = _whAffairAppService.DaySettle(DomainManager.GetDepotId(), id, 0);
            return ReturnJsonResult(ret);
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult DayCheckArticles(DateTime id)
        {
            string ret = _recordAppService.DaySettle(DomainManager.GetDepotId(), id, 0);
            return ReturnJsonResult(ret);
        }

        #region util
        private JsonResult ReturnJsonResult(string msg)
        {
            string result = null;
            int num = 0;
            if (int.TryParse(msg, out num))
                result = "ok";
            else
                result = "warn";
                
            return Json( new { result = result, content = msg });
        }
        #endregion
    }
}