using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class SysAUserRole:Entity
    {
        [Key]
        public int URId { get; set; }
        public string SysUserId { get; set; }
        public string SysRoleId { get; set; }
    }
}
