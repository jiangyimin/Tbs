using System.Collections.Generic;
using Abp.Application.Services;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IWeixinAppService : IApplicationService
    {
        WxIdentifyDto Login(int tenantId, string workerCn, string password, string deviceId);
 
        // List<RouteTaskDto> GetTaskList(string workerId);

        string Signin(int tenantId, string workerCn, double lon, double lat, double accuracy);

        void SetIdentifyTime(int taskId, int routeId, int outletId);

        void ResetDeviceId(int id);
        void ResetAllDeviceId();

        string[] ProcessTextMessage(int tenantId, string fromUser, string content);
    }
}
