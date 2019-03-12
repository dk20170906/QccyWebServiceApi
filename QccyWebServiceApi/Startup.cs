using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Events;
using EdaSample.EventBus.RabbitMQ;
using EdaSample.EventStores.Dapper;
using EdaSample.Integration.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QccyWebServiceApi.Common;
using QccyWebServiceApi.EF;
using QccyWebServiceApi.Models.System;
using RabbitMQ.Client;

namespace QccyWebServiceApi
{
    public class Startup
    {
        private const string RMQ_EXCHANGE = "EdaSample.Exchange";
        private const string RMQ_QUEUE = "EdaSample.CustomerServiceQueue";

        private readonly ILogger logger;
        private readonly string connStr;
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.Configuration = configuration;
            this.logger = loggerFactory.CreateLogger<Startup>();
            this.connStr = configuration["WebApiConfig:SqlConnectionString"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            this.logger.LogInformation("正在对服务进行配置...");

            #region 跨域
            var isTrueStr = Configuration["HttpKYUrls:isTrue"];
            var httpUrlStr = Configuration["HttpKYUrls:urlStr"];
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSameDomainHttp", builder =>
                {
                    if (isTrueStr.Equals("true") && !string.IsNullOrWhiteSpace(httpUrlStr))
                    {
                        this.logger.LogInformation("注册跨域请求，指定路由为：" + httpUrlStr);
                        builder.WithOrigins(httpUrlStr.Split(','))
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();//允许处理cookie
                    }
                    else
                    {
                        this.logger.LogInformation("注册跨域请求，允许所有主机访问");
                        builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()     //允许所有来源的主机访问
                .AllowCredentials();
                    }
                });
            });


            #endregion

            #region 用户登录验证


            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions(
               Configuration["WebApiConfig:JWTIssuer"],
                           Configuration["WebApiConfig:JWTAudience"],
                                       Configuration["WebApiConfig:JWTSecurityKey"],
                                        Convert.ToInt32(Configuration["WebApiConfig:JWTExpires"])
               );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //认证失败，会自动跳转到这个地址
                    options.LoginPath = "/Home/Login";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtTokenOptions.Key,

                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtTokenOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(Convert.ToInt32(Configuration["WebApiConfig:JWTClockSkew"]))
                    };
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion



            services.AddOptions();
            services.Configure<WebApiConfig>(Configuration.GetSection("WebApiConfig"));
            services.AddTransient<IEventStore>(serviceProvider =>
               new DapperEventStore(connStr,
                   serviceProvider.GetRequiredService<ILogger<DapperEventStore>>()));

            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services,
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);
            // services.AddSingleton<IEventBus, PassThroughEventBus>();
            services.AddDbContext<WebApiDbContext>(options => options.UseSqlServer(connStr));


            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            services.AddSingleton<IEventBus>(sp => new RabbitMQEventBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                sp.GetRequiredService<IEventHandlerExecutionContext>(),
                RMQ_EXCHANGE,
                queueName: RMQ_QUEUE));




            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            this.logger.LogInformation("服务配置完成，已注册到IoC容器！");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCors("AllowSameDomainHttp");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
