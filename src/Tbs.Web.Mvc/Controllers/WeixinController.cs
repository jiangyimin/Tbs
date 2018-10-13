using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Xml.Linq;
using Senparc.Weixin.Work;
using Senparc.Weixin.Work.Entities;
using Tbs.Web.MessageHandlers;
using Tbs.Controllers;
using Tbs.Configuration;
using Senparc.Weixin.Work.Containers;
using Tbs.DomainModels;

namespace Tbs.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class WeixinController : TbsControllerBase
    {
        private IWeixinAppService _weixinAppService;
        // public static readonly string Token = "Cl9m9V6BFmA07zTILr4N";
        // public static readonly string EncodingAESKey = "narOZYCLgorLAxW4yFhafXBqrX8DiCNmTDKwDYJiETT";
        // public static readonly string CorpId = "wx9b674b6377fc64be";
        public WeixinController(IWeixinAppService weixinAppService)
        {
            _weixinAppService = weixinAppService;
        }

        /// <summary> 
        /// Get index
        /// </summary> 
        [HttpGet] 
        [ActionName("Index")] 
        public ActionResult Get(string id, string msg_signature = "", string timestamp = "", string nonce = "", string echostr = "")
        {
            Logger.Warn($"微信Get {id}");
            int tenantId = DomainManager.GetTenantIdByName(id);
            if (tenantId == 0) {
                return Content("没有对应的公司编号");
            }
            string token = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Token, tenantId).Result;
            string aeskey = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.EncodingAESKey, tenantId).Result;
            string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, tenantId).Result;

            var verifyUrl = Signature.VerifyURL(token, aeskey, corpId, msg_signature, timestamp, nonce, echostr); 
            if (verifyUrl != null) 
            { 
                return Content(verifyUrl); 
            } 
            else 
            { 
                return Content("Error"); 
            } 
        } 
 
        /// <summary> 
        ///  
        /// </summary> 
        /// <param name="postModel"></param> 
        /// <returns></returns> 
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(string id, PostModel postModel) //string msg_signature = "", string timestamp = "", string nonce = "")
        { 
            int tenantId = DomainManager.GetTenantIdByName(id);
            if (tenantId == 0) {
                return Content("没有对应的公司编号");
            }

            string token = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Token, tenantId).Result;
            string aeskey = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.EncodingAESKey, tenantId).Result;
            string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, tenantId).Result;

            postModel.Token = token; 
            postModel.EncodingAESKey = aeskey; 
            postModel.CorpId = corpId;

            var maxRecordCount = 10;
            var inputStream = new MemoryStream();
            Request.Body.CopyTo(inputStream);
            try
            {
                var messageHandler = new WorkCustomMessageHandler(inputStream, postModel, maxRecordCount);
                messageHandler.WeixinAppService = _weixinAppService;
                messageHandler.TenantId = tenantId;
                messageHandler.OmitRepeatedMessage = true;
                Logger.Warn($"微信Request={messageHandler.TenantId} {messageHandler.WeixinAppService.ToString()}");
                messageHandler.Execute();

                // var ret = new Senparc.Weixin.MP.CoreMvcExtension.FixWeixinBugWeixinResult(messageHandler);
                // Logger.Warn($"微信Response={messageHandler.FinalResponseDocument.ToString()}");
                return Content(messageHandler.FinalResponseDocument.ToString(), "text/xml");
            }
            catch (Exception ex) {
                Logger.Error($"微信MessageHandle错误 {ex.Message}");
                return Content("");
            }
        } 
    }
}