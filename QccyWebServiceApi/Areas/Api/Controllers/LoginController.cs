using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EdaSample.Common.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QccyWebServiceApi.Common;
using QccyWebServiceApi.EF;
using QccyWebServiceApi.Models.System;

namespace QccyWebServiceApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IEventBus eventBus;
        private readonly ILogger logger;
        private readonly WebApiDbContext dbContext;
        private readonly WebApiConfig webApiConfig;

        public LoginController(
            IEventBus eventBus,
            ILogger<ILoggerFactory> logger,
         WebApiDbContext dbContext,
         IOptions<WebApiConfig> options)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.dbContext = dbContext;
            this.webApiConfig = options.Value;
        }
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery]SysAUser sysUser)
        {
            //    var user = await dbContext.SysUsers.FirstOrDefaultAsync(u => (u.UserName.Equals(sysUser.UserName) || u.Accounts == sysUser.UserName
            //|| u.Email == sysUser.UserName || u.EobilePhone == sysUser.UserName)
            //&& u.Password == sysUser.Password);
            var user = new SysAUser() { UserName = "123", Password = "12" };
            if (user != null)
            {
                return new JsonResult(new RespResult
                {
                    Code = (int)RespState.SUCCESS,
                    Msg = "登录成功",
                    Data = GetToken(user.UserName)
                });
            }
            return new JsonResult(new RespResult
            {

            });
        }
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(Guid id)
        {
            return new JsonResult(new RespResult
            {

            });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Post([FromBody]SysAUser sysUser)
        {
            return new JsonResult(new RespResult
            {
                Code = (int)RespState.SUCCESS,
                Msg = "",
                Data = new SysAUser
                {
                    UserName = sysUser.UserName,
                    Password = sysUser.Password,
                    Access_token = GetToken(sysUser.UserName)
                }
            });
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public string GetToken(string username)
        {
            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions(webApiConfig.JWTIssuer, webApiConfig.JWTAudience, webApiConfig.JWTSecurityKey, webApiConfig.JWTExpires);

            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Sid, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user"),
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: jwtTokenOptions.Issuer,
                audience: jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(webApiConfig.JWTExpires),
                signingCredentials: jwtTokenOptions.Credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
