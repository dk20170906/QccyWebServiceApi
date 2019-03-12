using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class SysARoleMenu:Entity
    {
        [Key]
        public int RMId { get; set; }
        public string SysRoleId { get; set; }
        public string SysMenuId { get; set; }
    }
}
