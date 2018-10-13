using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Tbs.Authorization.Roles;

namespace Tbs.Roles.Dto
{
    [AutoMapTo(typeof(Role))]
    public class CreateRoleInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}