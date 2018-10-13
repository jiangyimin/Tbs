using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Data)]
    public class WorkersController : TbsCrudController<Worker, WorkerDto>
    {
        public WorkersController(IRepository<Worker> repository)
            :base(repository)
        {
        }

        public ActionResult GetPhoto(int id)
        {
            Worker v = Get(id);
            if (v != null) {
                if (v.Photo != null)
                    return File(v.Photo, "image/jpg");
                else
                    return File(new byte[0], "image/jpg");
            }
            else
                throw new UserFriendlyException("无此人员");

        }
	}
}