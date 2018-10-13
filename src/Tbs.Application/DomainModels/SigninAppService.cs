using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Tbs.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using System;
using Tbs.DomainServices;
using Abp.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Tbs.DomainModels
{
    [AbpAuthorize(PermissionNames.DispatcherPages)]
    public class SigninAppService : TbsAppServiceBase, ISigninAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<DepotSignin> _depotSigninRepository;
        private readonly IRepository<Signin> _signinRepository;

        private readonly SigninDomainService _signinService;

        public SigninAppService(IRepository<DepotSignin> depotSigninRepository, IRepository<Signin> signinRepository, 
            SigninDomainService signinService)
        {
            _depotSigninRepository = depotSigninRepository;
            _signinRepository = signinRepository;
            _signinService = signinService;
        }

        public List<DepotSigninDto> GetDepotSignins(int depotId)
        {
            var signins = _depotSigninRepository.GetAllList( e => e.DepotId == depotId);
            return new List<DepotSigninDto>(
                ObjectMapper.Map<List<DepotSigninDto>>(signins)
            );
        }

        public string GetAgentInfo(int depotId, DateTime carryoutDate)
        {
            var entity = _signinRepository.FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == carryoutDate && s.Name == "代理");
            if (entity != null)
                return $"由代理人({entity.Worker})";
            
            return null;
        }
        public List<SigninListDto> GetSignins(int depotId, DateTime carryoutDate, string name)
        {
            var signins = _signinRepository.GetAllList( e => 
                e.DepotId == depotId && e.CarryoutDate == carryoutDate && e.Name == name);

            return new List<SigninListDto>(
                    ObjectMapper.Map<List<SigninListDto>>(signins.OrderBy(t => t.Worker))
            );
        }

        public List<SigninListDto> GetSignins(int depotId, string name)
        {
            var signins = _signinRepository.GetAllList( e => e.DepotId == depotId && e.Name == name);

            return new List<SigninListDto>(
                    ObjectMapper.Map<List<SigninListDto>>(signins.OrderByDescending(t => t.SigninTime))
            );
        }

        public string SigninByIdCardNo(int depotId, string idCardNo, bool agent)
        {
            foreach (Worker w in DomainManager.GetWorkers(depotId))
            {
                if (w.IDCardNo == idCardNo)
                {
                    return _signinService.Signin(w, agent);
                }
            }
            return "找不到此ID卡号的员工";
        }

        public async Task<List<SigninStatDto>> Stat(int depotId, DateTime begin, DateTime end)
        {
            if (end.Subtract(begin) > TimeSpan.FromDays(90))
                throw new Abp.UI.UserFriendlyException("日期不能超过90天!");
                
            var query = _signinRepository.GetAll().Where(a => a.DepotId == depotId && 
                a.CarryoutDate >= begin && a.CarryoutDate <= end);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            Dictionary<string, int[]> dict = new Dictionary<string, int[]>();

            List<DepotSigninDto> names = GetDepotSignins(depotId);          
            foreach (var s in entities)
            {
                if (!dict.ContainsKey(s.Worker)) 
                    dict[s.Worker] = new int[9];
                
                int[] vs = dict[s.Worker];                
                int i = GetIndexByName(s.Name, names); 
                if (i < 0 || i > 2) continue;
                vs[i * 3] += 1;
                if (s.LateDistance > 0) {
                    vs[i * 3 + 1] += 1;
                    vs[i * 3 + 2] += s.LateDistance;
                }
            }

            var dtos = new List<SigninStatDto>();
            foreach (var item in dict.ToList().OrderBy(a => a.Key))
            {
                SigninStatDto dto = new SigninStatDto(item.Key);
                int[] vs = item.Value;
                for (int i = 0; i < 3; i++)
                {
                    PropertyInfo propDay = typeof(SigninStatDto).GetProperty($"DayCount{i+1}");
                    PropertyInfo propLate = typeof(SigninStatDto).GetProperty($"LateCount{i+1}");
                    PropertyInfo propTotal = typeof(SigninStatDto).GetProperty($"LateTotal{i+1}");

                    propDay.SetValue(dto, vs[i * 3]);
                    propLate.SetValue(dto, vs[i * 3 + 1]);
                    propTotal.SetValue(dto, vs[i * 3 + 2]);                    
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        private int GetIndexByName(string name, List<DepotSigninDto> names)
        {
            for (int i = 0; i < names.Count; i++)
                if (name == names[i].Name) 
                    return i;
            return -1;
        }
    }
}