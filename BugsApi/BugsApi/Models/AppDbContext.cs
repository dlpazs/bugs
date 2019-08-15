using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BugsApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<UsersModel> User { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

    }
}
