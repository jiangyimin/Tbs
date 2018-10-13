using System.IO;
using System.Xml.Linq;
using Castle.Core.Logging;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using Tbs.DomainModels;

namespace Tbs.Web.MessageHandlers
{
    public class WorkCustomMessageHandler : WorkMessageHandler<WorkCustomMessageContext>
    {
        public int TenantId { get; set; }
        public IWeixinAppService WeixinAppService { get; set; }
        public WorkCustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
        }

        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";
            return responseMessage;
        }
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        { 
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            
            string[] ret = WeixinAppService.ProcessTextMessage(TenantId, requestMessage.FromUserName, requestMessage.Content);
            responseMessage.Content = ret[0];
            
            if (ret[1] != null)
                WeixinUtils.SendMessage(TenantId, ret[1], ret[2]);          

            return responseMessage; 
        } 
 
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        { 
            var responseMessage = CreateResponseMessage<ResponseMessageImage>(); 
            responseMessage.Image.MediaId = requestMessage.MediaId; 
            return responseMessage; 
        } 
 
        // public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        // { 
        //     var responseMessage = this.CreateResponseMessage<ResponseMessageText>(); 
        //     responseMessage.Content = "您刚发送的图片如下："; 
        //     return responseMessage; 
        // } 
 
        // public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        // { 
        //     var responseMessage = this.CreateResponseMessage<ResponseMessageText>(); 
        //     responseMessage.Content = string.Format("位置坐标 {0} - {1}", requestMessage.Latitude, requestMessage.Longitude); 
        //     return responseMessage; 
        // } 
 
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        { 
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>(); 
            responseMessage.Content = "这是一条没有找到合适回复信息的默认消息。"; 
            return responseMessage; 
        } 
    }
}
