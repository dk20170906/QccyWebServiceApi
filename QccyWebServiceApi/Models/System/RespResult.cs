using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class RespResult
    {
        public int Code { get; set; }
        public bool Success { get; set; } = true;
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
