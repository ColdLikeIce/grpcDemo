using System;
using System.Collections.Generic;
using System.Text;

namespace Hy.FantasyEFCore.DbDao
{
    public class FantasyDbContext : DbContext
    {
        public DbSet<GameAccount> GameAccount { get; set; }

        public FantasyDbContext(DbContextOptions<FantasyDbContext> options)
            : base(options)
        {
        }
    }
}