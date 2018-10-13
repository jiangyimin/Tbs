using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Tbs.Authorization.Users;
using Tbs.DomainModels;
using Tbs.MultiTenancy;

namespace Tbs.DomainServices
{
    /// <summary>
    /// DepotSignin Servicer.
    /// </summary>
    public class SigninDomainService : IDomainService
    {
        private readonly IRepository<Signin> _signinRepository;
        private readonly IDepotSigninCache _depotSigninCache;
        public SigninDomainService(
            IRepository<Signin> signinRepository,
            IDepotSigninCache depotSigninCache)
        {
            _signinRepository = signinRepository;
            _depotSigninCache = depotSigninCache;
        }
        
        public string Signin(Worker worker, bool agent = false)
        {
            string workerInfo = $"{worker.Cn} {worker.Name}";

            if (agent == true)
            {
                var entity = _signinRepository.FirstOrDefault(s => s.DepotId == worker.DepotId && s.CarryoutDate == DateTime.Today && s.Name == "代理");
                if (entity == null) {
                    entity = new Signin();
                    entity.CarryoutDate = DateTime.Today;
                    entity.DepotId = worker.DepotId;
                    entity.Name = "代理";
                    entity.Worker = workerInfo;
                    entity.SigninTime = DateTime.Now;
                    _signinRepository.Insert(entity);
                    return $"代理（{workerInfo}）登记成功";
                }

                return $"已有代理（{entity.Worker}）已登记过";
            }

            // get the depotsignin
            var depotSignin = GetSignin(worker.DepotId);
            if (depotSignin == null)
                return "非签到时段";
 
            // 求迟到分钟
            int late = Tbs.Timing.TimeUtil.DistToNow(DateTime.Today, depotSignin.LateTime);
            if (late < 0) late = 0;

            // Get Signin 
            var signin = _signinRepository.FirstOrDefault(s => s.DepotId == worker.DepotId && s.CarryoutDate == DateTime.Today 
                    && s.Name == depotSignin.Name && s.Worker == workerInfo);
            if (signin == null)
            {
                Signin s = new Signin() { TenantId = worker.TenantId, DepotId = worker.DepotId, CarryoutDate = DateTime.Today, Name = depotSignin.Name, 
                        Worker = workerInfo, SigninTime = DateTime.Now, LateDistance = late };
                _signinRepository.Insert(s);

                if (late == 0)
                    return $"{worker.Name}签到成功！";
                else
                    return $"{worker.Name}签到成功(迟到{late}分钟)";
            }
            else
            {
                return "你已签过到";
            }
        }

        public bool IsSignin(Worker worker)
        {
            DepotSignin ds = GetSignin(worker.DepotId);
            if (ds == null) return true;

            string workerInfo = $"{worker.Cn} {worker.Name}";
            var signin = _signinRepository.FirstOrDefault(s => s.DepotId == worker.DepotId && s.CarryoutDate == DateTime.Today 
                    && s.Name == ds.Name && s.Worker == workerInfo);
            return signin != null;            
        }

        private DepotSignin GetSignin(int depotId)
        {
           var lst = _depotSigninCache.GetList(depotId);
           foreach (DepotSignin s in lst)
           {
                if (Tbs.Timing.TimeUtil.IsIn(DateTime.Today, s.StartTime, s.EndTime))
                     return s;
           }
           return null;
        }

        
    }
}