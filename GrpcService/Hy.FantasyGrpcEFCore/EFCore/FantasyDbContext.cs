using Hy.FantasyGrpcEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hy.FantasyGrpcEFCore.EFCore
{
    public class FantasyDbContext : DbContext
    {
        public DbSet<GameAccount> GameAccount { get; set; }

        public FantasyDbContext(DbContextOptions<FantasyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFantasy();
        }
    }
}