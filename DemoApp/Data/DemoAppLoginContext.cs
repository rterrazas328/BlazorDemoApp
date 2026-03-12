using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;



namespace DemoApp.Data
{
    //Change to DBIdentityContext later
    public class DemoAppLoginContext : DbContext
    {
        public DemoAppLoginContext(DbContextOptions<DemoAppLoginContext> options)
            : base(options)
        {
            
        }

        public DbSet<UserAccount> UserLogins { get; set; } = default!;
    }
}
