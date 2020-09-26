using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MangoServer.Model;

namespace MangoServer.Data
{
    public class MangoServerContext : DbContext
    {
        public MangoServerContext (DbContextOptions<MangoServerContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Item> Items { get; set; }

    }
}
