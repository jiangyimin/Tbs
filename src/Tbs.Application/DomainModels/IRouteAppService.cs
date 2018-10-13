using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public class ArticleLended
    {
        public int Id { get; set; }
        public string Cn { get; set; }
        public string Name { get; set; }

        public string TypeName { get; set; }

        public string Rfid { get; set; }

        public ArticleLended(int id, string cn, string name, string typeName, string rfid)
        {
            Id = id;
            Cn = cn;
            Name = name;
            TypeName = typeName;
            Rfid = Rfid;
        }
    }

    public class WorkerMatched
    {
        public int RouteWorkerId { get; set; }
        public string RecordList { get; set; }

        public List<ArticleLended> ArticlesLended { get; set; }

        public int Id { get; set; }
        public string Cn { get; set; }
        public string Name { get; set; }

        public string Photo { get; set; }
        public string IdCardNo { get; set; }

        public int RouteRoleId { get; set; }
        public string RouteRoleName { get; set; }

        public WorkerMatched(int routeWorkerId, string recordList, int id, string cn, string name, string idCardNo, byte[] photo, int routeRoleId, string routeRoleName)
        {
            RouteWorkerId = routeWorkerId;
            RecordList = recordList;
            ArticlesLended = new List<ArticleLended>();
            Id = id;
            Cn = cn;
            Name = name;
            IdCardNo = idCardNo;
            Photo = photo != null ? Convert.ToBase64String(photo) : null;

            RouteRoleId = routeRoleId;
            RouteRoleName = routeRoleName;
        }
    }

    public class RouteWorkerMatchResult {
        public string Result { get; set; }
        public string Message { get; set; }
        public int  RouteId { get; set; }

        public string RouteCn { get; set; }
        public string VehicleCn { get; set; }

        public List<WorkerMatched> Workers { get; set; }

        public RouteWorkerMatchResult(string message)
        {
            Result = "error";
            Message = message;
        }
        public RouteWorkerMatchResult(int routeId)
        {
            Result = "ok";
            RouteId = routeId;
        }

        public void AddWorker(WorkerMatched w)
        {
            if (Workers == null) Workers = new List<WorkerMatched>();
            Workers.Add(w);
        }
    }

    public interface IRouteAppService : IApplicationService
    {
        List<RouteDto> GetRoutes(int depotId, DateTime carryoutDate, string sorting);
        List<RouteIdentifyDto> GetRouteIdentifies(int routeId, string sorting);
        List<RouteDto> GetRoutesActive(int depotId, DateTime carryoutDate, string sorting);
        RouteWorkerMatchResult FindWorkersForArticle(int depotId, DateTime carryoutDate, Worker worker, string style);

        Route FindRouteForIdentify(int depotId, int workerId, out int subWorkerId);
        void SetIdentifyTime(int taskId, int routeId, int outletId);

        Task<RouteDto> Insert(RouteDto input);
        Task<RouteDto> Update(RouteDto input);
        Task Delete(int id);

        List<RouteWorkerDto> GetRouteWorkers(int id, string sorting);
        Task<RouteWorkerDto> InsertSon(RouteWorkerDto input);
        Task<RouteWorkerDto> UpdateSon(RouteWorkerDto input);
        Task DeleteSon(int id);

        List<RouteTaskDto> GetRouteTasks(int id, string sorting);
        Task<RouteTaskDto> InsertSon2(RouteTaskDto input);
        Task<RouteTaskDto> UpdateSon2(RouteTaskDto input);
        Task DeleteSon2(int id);

        List<RouteDto> CheckActivate(int depotId, DateTime carryoutDate);
        Task<int> Activate(int depotId, DateTime carryoutDate, string status);
        Task<int> ActivateSelects(List<int> ids, string status);

        Task<int> CreateFromPre(int depotId, DateTime carryoutDate);
        Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate);

        string DaySettle(int depotId, DateTime carryoutDate, int settleId);
        Task<List<RouteWorkerStatDto>> Stat(int depotId, DateTime begin, DateTime end);

    }
}