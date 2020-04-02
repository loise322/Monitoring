using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace Monitoring.Models
{
    public class TableContext : DbContext
    {
        public DbSet<LogObject> Logs { get; set; }
        public DbSet<MetricItem> Metrics { get; set; }

        public TableContext(DbContextOptions<TableContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
