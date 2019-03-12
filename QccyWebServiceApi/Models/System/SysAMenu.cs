using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class SysMenu : Entity
    {
        [Key]
        public int MenuId { get; set; }
        public string PId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Jump { get; set; }
        public List<SysMenu> List { get; set; }
    }
}
