using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QccyWebServiceApi.EF;
using QccyWebServiceApi.Models.System;

namespace QccyWebServiceApi.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEventBus eventBus;
        private readonly ILogger logger;
        private readonly WebApiDbContext dbContext;
        private readonly WebApiConfig webApiConfig;
        public UserController(IEventBus eventBus,
            ILogger<ILoggerFactory> logger,
            WebApiDbContext dbContext,
             IOptions<WebApiConfig> option)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.dbContext = dbContext;
            this.webApiConfig = option.Value;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]SysAUser sysUser)
        {
            var user = await dbContext.AddAsync<SysAUser>(sysUser);
            if (user.State.Equals(EntityState.Added))
            {
                return new JsonResult(new RespResult
                {
                    Code = (int)RespState.SUCCESS,
                    Msg = RespState.SUCCESS.ToString(),
                    Data = user.Entity
                });
            }
            return new JsonResult(new RespResult
            {
                Code = (int)RespState.ERROR,
                Msg = RespState.ERROR.ToString()
            });

        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
