using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Demo.Core.Model.Demo;

namespace Demo.Core.Models
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<Demo.Core.Model.Demo.DemoInfo> DemoInfo { get; set; }
    }
}
