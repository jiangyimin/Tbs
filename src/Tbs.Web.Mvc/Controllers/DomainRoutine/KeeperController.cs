using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Tbs.Web.Models;
using Tbs.DomainModels;
using Abp.UI;
using System;
using System.Linq;
using Tbs.DomainServices;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Tbs.DomainModels.Dto;
using System.Threading.Tasks;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize]
    public class KeeperController : TbsControllerBase
    {
        private readonly IWhAffairAppService _whAffairAppService;
        private readonly IVtAffairAppService _vtAffairAppService;
        private readonly IRouteAppService _routeAppService;
        private readonly IRepository<Vault> _vaultRespository;

        private readonly IRecordAppService _recordAppService;

        public KeeperController(IRepository<Vault> vaultRepository, IWhAffairAppService whAffairAppService,
            IVtAffairAppService vtAffairAppService, IRouteAppService routeAppService, IRecordAppService recordAppService)
        {
            _vaultRespository = vaultRepository;
            _whAffairAppService = whAffairAppService;
            _vtAffairAppService = vtAffairAppService;
            _routeAppService = routeAppService;
            _recordAppService = recordAppService;
        }

        #region Article
        public ActionResult Index()
        {
            int depotId = DomainManager.GetDepotId();            
            return View(depotId);
        }

        [DontWrapResult]
        public async Task<JsonResult> ArticleListGridData(int id)
        {
            var output = await _recordAppService.GetArticleList(id, GetSorting());
            return Json(new { rows = output });
        }

        public ActionResult ArticleRecords()
        {
            return View(DomainManager.GetCurrentUserDepots());
        }
        
        #endregion

        #region WhAffairs
        public ActionResult WhAffairsCheck()
        {
            int depotId = DomainManager.GetDepotId();
            string keepers = null;            
            Warehouse myWh = CheckWhAffair(depotId, ref keepers); 
            var vm = new KeeperViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                Warehouse = myWh,
                Keepers = keepers
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult WhAffairsGridData(int depotId, DateTime carryoutDate, int whId)
        {
            var wh = DomainManager.GetWarehouses(depotId).FirstOrDefault(w => w.Id == whId);
            var output = _whAffairAppService.GetAffairsActive(depotId, carryoutDate, wh.Name, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult VtAffairWorkersGridData(int id)
        {
            var output = _vtAffairAppService.GetAffairWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        #endregion
        #region vtAffairs
        public ActionResult VtAffairsCheck()
        {
            int depotId = DomainManager.GetDepotId();            
            string keepers = null;            
            Warehouse myWh = CheckWhAffair(depotId, ref keepers); 
            
            int affairId = 0;
            if (keepers != null) {
                string[] strs = keepers.Split(',');
                affairId = int.Parse(strs[0]);
                keepers = strs[1];
            }
                
            var vm = new KeeperViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                Warehouse = myWh,
                Keepers = keepers,
                WhAffairId = affairId
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult VtAffairsGridData(int warehouseId, DateTime carryoutDate)
        {
            List<Vault> lst = _vaultRespository.GetAllList(v => v.WarehouseId == warehouseId);
            var output = new List<VtAffairDto>();
            foreach (Vault v in lst)
            {
                output.AddRange(_vtAffairAppService.GetAffairsActive(v.DepotId, carryoutDate, GetSorting()));
            }
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult WhAffairWorkersGridData(int id)
        {
            var output = _whAffairAppService.GetAffairWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        #endregion

        #region RoutesCheck
        public ActionResult RoutesArticle()
        {
            int depotId = DomainManager.GetDepotId();            
            string keepers = null;            
            Warehouse myWh = CheckWhAffair(depotId, ref keepers); 
            var vm = new KeeperViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                Depots = GetDepotsManaged(depotId, myWh),
                Keepers = keepers
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult RoutesGridData(int depotId, DateTime carryoutDate)
        {
            var output = _routeAppService.GetRoutesActive(depotId, carryoutDate, GetSorting());   
            foreach (var o in output) {
                if (o.VehicleId.HasValue) {
                    Vehicle v= DomainManager.GetVehicle(depotId, o.VehicleId.Value);
                    o.VehicleInfo = v != null ? $"{v.Cn} {v.License}" : null;
                }
            }         
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult RouteWorkersGridData(int id)
        {
            var output = _routeAppService.GetRouteWorkers(id, GetSorting());
            foreach (var o in output) {
                o.RecordList = DomainManager.GetRecordListDetail(o.RecordList);
            }
            return Json( new { rows = output });
        }

        #endregion

        #region HttpPost
        [HttpPost]
        [DontWrapResult]        
        public JsonResult MatchRouteWorkerIdCardNo(int depotId, DateTime carryoutDate, string style, string idCardNo)
        {
            foreach (Worker w in DomainManager.GetWorkers(depotId))
            {
                if (w.IDCardNo == idCardNo)
                {
                    var output = _routeAppService.FindWorkersForArticle(depotId, carryoutDate, w, style);
                    return Json(output);
                }
            }
            return Json(new { result = "error", message = "此ID卡找不到对应的人员"});
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult MatchArticleRfid(int style, int depotId, string rfid, int routeRoleId)
        {
            foreach (Article a in DomainManager.GetArticles(depotId))
            {
                var article = DomainManager.GetArticle(depotId, a.Id);
                if (!string.IsNullOrEmpty(a.Rfid) && a.Rfid == rfid)
                {
                    return CheckThisArticle(style, a, routeRoleId);
                }
            }
            return Json(new { result = "error", message = "找不到此Rfid的物品"});
        }

        [HttpPost]
        [DontWrapResult]     
        public JsonResult CheckArticles(int style, int depotId, int routeId, int routeWorkerId, int workerId, string workerCn, string workerName, List<int> ids)
        {
            try 
            {
                Worker uw = DomainManager.GetUserWorker();
                string remark = uw != null ? uw.Cn + " " + uw.Name : null;
                if (style == 1)
                    _recordAppService.LendArticles(depotId, routeId, routeWorkerId, workerId, workerCn, workerName, ids, remark);
                else
                    _recordAppService.ReturnArticles(routeId, routeWorkerId, remark);
                string method = (style == 1 ? "领用" : "归还");
                return Json(new { result = "ok", message = $"{workerName}{method}物品成功"});
            }
            catch (Exception ex)
            {
                Logger.Error("物品" + ex.Message);
                return Json(new { result = "error", message = $"发生内部错误"});
            }
        }

        [HttpPost]
        [DontWrapResult]       
        public JsonResult MatchIdCardNo(int style, int depotId, int affairId, string idCardNo)
        {
            foreach ( Worker w in DomainManager.GetWorkers(depotId))
            {
                if (w.IDCardNo == idCardNo)
                {
                    if (style == 1)
                        return CheckWhAffairWorker(w, affairId);
                    else if (style == 2)
                        return CheckVtAffairWorker(w, affairId);
                    else
                        return MatchWhAffairWorker(w, affairId);
                }
            }
            return Json(new { result = "error", message = "此ID卡找不到对应的人员"});
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult MatchFinger(int style, int depotId, int affairId, string finger)
        {
            Logger.Debug($"Enter MatchFinger {finger.Length}");
            byte[] src = StringToByte(finger, 0);
            int maxScore = 0;
            foreach (Worker w in DomainManager.GetWorkers(depotId))
            {
                if (string.IsNullOrEmpty(w.Finger))
                    continue;
                byte[] dst = StringToByte(w.Finger, 0);
                byte[] dst1 = StringToByte(w.Finger, 1);

                int score=0, score1=0;
                try {
                    int ret = FingerDll.UserMatch(src, dst, 3, ref score);
                    int ret1 = FingerDll.UserMatch(src, dst1, 3, ref score1);
                }
                catch (Exception ex) {
                    Logger.Debug(ex.Message);
                }

                int matchScore = Math.Max(score, score1);
                if (maxScore < matchScore)
                    maxScore = matchScore;

                if (matchScore > 50)
                {
                    if (style == 1)
                        return CheckWhAffairWorker(w, affairId);
                    else
                        return CheckVtAffairWorker(w, affairId, matchScore);
                }

            }
            return Json(new { result = "error", message = $"此指纹匹配不到(最大分值 {maxScore}" } );
        }

        #endregion

        #region util
        private Warehouse CheckWhAffair(int depotId, ref string keepers)
        {
            var user = DomainManager.GetCurrentUser();
            Warehouse wh = DomainManager.GetWarehouses(depotId).FirstOrDefault(w=>w.Name == user.WhName);
            if (string.IsNullOrEmpty(user.WhName) || wh == null)
                 throw new UserFriendlyException("请系统管理员设置用户所在正确的库房名称");

            keepers = DomainManager.GetKeepersInfo(depotId, DateTime.Today, user.WhName);
            if (string.IsNullOrEmpty(keepers))
                 throw new UserFriendlyException("请调度安排并激活有对应时段的库房任务");

            if (!keepers.Contains(user.UserName))
                 throw new UserFriendlyException("登录用户未被安排为库房操作人员");
                
            return wh;
        }

        private byte[] StringToByte(string str, int j)
        {  
            byte[] outBytes = new byte[256];
            if (str.Length < (j + 1) * 512)
                return outBytes;
              
            for (int i = 0; i < outBytes.Length; i++)  
            {  
                outBytes[i] = Convert.ToByte(str.Substring(j*512 + i*2, 2), 16);  
            }  
            return outBytes;  
        }   

        private JsonResult MatchWhAffairWorker(Worker worker, int affairId, int score = 0)
        {
            KeyValuePair<string, string> ret =  _whAffairAppService.MatchWorker(worker, affairId);
            if (ret.Key == "ok")
            {
                return Json(new {
                    result = ret.Key,
                    message = ret.Value + $"(分值{score})",
                    workerCn = worker.Cn,
                    workerName = worker.Name,
                    workerPhoto = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null
                });
            }
            else
            {
                return Json(new {
                    result = ret.Key,
                    message = ret.Value + $"(分值{score})"
                });
            }
        }
        private JsonResult CheckWhAffairWorker(Worker worker, int affairId, int score = 0)
        {
            KeyValuePair<string, string> ret =  _whAffairAppService.CheckWorker(worker, affairId);
            if (ret.Key == "ok")
            {
                return Json(new {
                    result = ret.Key,
                    message = ret.Value + $"(分值{score})",
                    workerCn = worker.Cn,
                    workerName = worker.Name,
                    workerPhoto = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null
                });
            }
            else
            {
                return Json(new {
                    result = ret.Key,
                    message = ret.Value + $"(分值{score})"
                });
            }
        }
        private JsonResult CheckVtAffairWorker(Worker worker, int affairId, int score = 0)
        {
            KeyValuePair<string, string> ret =  _vtAffairAppService.CheckWorker(worker, affairId);
            if (ret.Key == "ok")
            {
                string checkstatus = null;
                if (ret.Value.Substring(0, 1) == "-")
                    checkstatus = "-";
                else if (ret.Value.Substring(0, 1) == "+")
                    checkstatus = "+";

                return Json(new {
                    result = ret.Key,
                    message = ret.Value + $"(分值{score})",
                    checkstatus = checkstatus,
                    workerCn = worker.Cn,
                    workerName = worker.Name,
                    workerPhoto = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null
                });
            }
            else
            {
                return Json(new {
                    result = ret.Key,
                    message = ret.Value + ($"(分值{score})")
                });
            }
        }
        private JsonResult CheckThisArticle(int style, Article article, int routeRoleId)
        {
            string msg = null;
            if (style == 1 && _recordAppService.ArticleLendedToday(article.Id, out msg))
            {
                return Json(new {
                    result = "error",
                    message = msg
                });
            }
            var type = DomainManager.GetArticleType(article.ArticleTypeId);
            var role = DomainManager.GetRouteRole(routeRoleId);
            if (role.ArticleTypeList.Contains(type.Cn))
            {
                return Json(new {
                    result = "ok",
                    message = string.Empty,
                    id = article.Id, 
                    cn = article.Cn,
                    name = article.Name,
                    rfid = article.Rfid,
                    bindTo = type.BindTo,
                    bindInfo = article.BindInfo
                });
            }
            else
            {
                return Json(new {
                    result = "error",
                    message = "你的角色不允许领此类物品"
                });
            }
        }

        private List<KeyValuePair<int, string>> GetDepotsManaged(int depotId, Warehouse wh)
        {            
            List<KeyValuePair<int, string>> lst = new List<KeyValuePair<int, string>>();
            lst.Add(new KeyValuePair<int, string>(depotId, DomainManager.GetDepotById(depotId).Name));

            if (string.IsNullOrEmpty(wh.DepotList))
                return lst;

            foreach (string cn in wh.DepotList.Split('|'))
            {
                Depot depot = DomainManager.GetDepotByCn(cn.Trim());
                if (depot != null)
                    lst.Add(new KeyValuePair<int, string>(depot.Id, depot.Name));
            }
            return lst;
        }
        #endregion 
    }
}