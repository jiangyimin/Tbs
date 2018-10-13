using System.ComponentModel.DataAnnotations;

namespace Tbs.Web.Models
{
    public class WxLoginViewModel
    {
        public string WorkerCn { get; set; }

        public string Password { get; set; }

        public string DeviceId { get; set; }

        public int TenantId { get; set; }
        public string ReturnUrl { get; set; }
    }
}