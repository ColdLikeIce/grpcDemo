using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Hy.FantasyGrpcEFCore
{
    public class DaoEnv
    {
        public static IConfiguration AppSetting { get; }

        private static string _connStr = null;

        public static string GetConnStr()
        {
            if (_connStr == null)
            {
                _connStr = DaoEnv.AppSetting["HyConnStr"];
            }
            return _connStr;
        }

        static DaoEnv()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}