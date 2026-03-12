using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;

namespace DemoApp.Data
{
    public class DemoAppContext : DbContext
    {
        public DemoAppContext (DbContextOptions<DemoAppContext> options)
            : base(options)
        {
        }

        public DbSet<DemoApp.Models.Employee> Employee { get; set; } = default!;
        
    }
}
