using Hy.FantasyGrpcEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hy.FantasyGrpcEFCore.EFCore
{
    public static class FantasyDbContextModelCreatingExtensions
    {
        private static ILogger? _logger;

        public static void ConfigureFantasy(this ModelBuilder builder)
        {
            if (builder == null || nameof(builder) == null)
            {
                return;
            }

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(FantasyConsts.DbTablePrefix + "YourEntities", FantasyConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            //builder.AutoMap<GameAccount>();
        }
    }
}