using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public class WebApiConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string SqlConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int JWTClockSkew { get; set; }//
        /// <summary>
        /// 发送方
        /// </summary>
        public string JWTIssuer { get; set; }
        /// <summary>
        /// 接收方
        /// </summary>
        public string JWTAudience { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string JWTSecurityKey { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int JWTExpires { get; set; }

    }
}
