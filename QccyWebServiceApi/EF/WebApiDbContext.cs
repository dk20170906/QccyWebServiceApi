using Microsoft.EntityFrameworkCore;
using QccyWebServiceApi.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.EF
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }
        public DbSet<SysAUser> SysAUsers { get; set; }
        public DbSet<SysMenu> SysAMenus { get; set; }
        public DbSet<SysARole> SysARoles { get; set; }
        public DbSet<SysARoleMenu> SysARoleMenus { get; set; }
        public DbSet<SysAUserRole> SysAUserRoles { get; set; }
    }
}
