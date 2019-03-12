using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class SysARole : Entity
    {
        public SysARole() { }
        public SysARole(Guid guid, string roleName) : base(guid)
        {
            this.RoleName = roleName;
        }
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string PId { get; set; }


    }
}
