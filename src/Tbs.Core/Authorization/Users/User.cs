using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using Tbs.DomainModels;

namespace Tbs.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        
        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }

        // added
        public const int MaxOperatePasswordLength = 20;
        /// <summary>
        /// 是否属于物流中心的
        /// </summary>
        public bool DepotSide { get; set; }

        /// <summary>
        /// 对应的单个角色名
        /// </summary>
        [StringLength(Abp.Authorization.Roles.AbpRoleBase.MaxNameLength)]
        public string RoleName { get; set; }

        [StringLength(Warehouse.MaxNameLength)]
        public string WhName { get; set; }
        
        /// <summary>
        /// OperatePassword
        /// </summary>
        [StringLength(MaxOperatePasswordLength)]
        public string OperatePassword { get; set; }


        public string GetOpPassword() {
            if (OperatePassword.Length >= 6)
                return OperatePassword.Substring(0, 6);
            else
                return null;
        }

        public string GenOpPassword() {
            int seed = (int)DateTime.Now.Ticks;
            Random r = new Random(seed);
            string ret = string.Empty;
            for (int i = 0; i < 6; i++)  {
                ret += r.Next(10).ToString();
            }
            return ret;
        }
        public void SetOpPassword(string pwd, int span) {
            DateTime dt = DateTime.Now.AddHours(span);

            OperatePassword = pwd + " " + dt.ToString("yyyy-MM-dd HH");
        }

        public DateTime GetOpDeadline()
        {
            DateTime ret;
            if (OperatePassword.Length < 17)
                return DateTime.MaxValue;

            if (!DateTime.TryParse(OperatePassword.Substring(7, 10), out ret))
                return DateTime.MaxValue;

            int hours = 0;
            if (OperatePassword.Length == 20 && int.TryParse(OperatePassword.Substring(18, 2), out hours))
                ret = ret.AddHours(hours);

            return ret; 
        }
    }
}