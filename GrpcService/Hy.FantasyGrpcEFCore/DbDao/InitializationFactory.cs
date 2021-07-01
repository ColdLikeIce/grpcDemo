using Hy.FantasyGrpcEFCore.EFCore;
using Hy.FantasyGrpcEFCore.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace Hy.FantasyGrpcEFCore.DbDao
{
    public static class InitializationFactory
    {
        /// <summary>
        /// 初始化工厂
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void Injection(IServiceCollection services, IConfiguration Configuration)
        {
            string connStr = Configuration.GetConnectionString("Default");
            string DIMethod = Configuration.GetValue<string>("DIMethod");
            services.AddDbContext<FantasyDbContext>(options =>
            {
                options.UseMySql(connStr);
            });
            InjectionRepositorys(services, DIMethod);
            InjectionServices.Injection(services, "Hy.FantasyGrpcEFCore", (ServiceLifetime)Enum.Parse(typeof(ServiceLifetime), DIMethod));
            //try
            //{
            //    //检查是否连接到数据库
            //    DbConnection dbConnection = new SqlConnection();
            //    dbConnection.ConnectionString = connStr;
            //    var cmd = dbConnection.CreateCommand();
            //    dbConnection.Open();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("数据库配置错误请检查：" + ex.ToString());
            //}
        }

        /// <summary>
        /// 依赖注入仓储
        /// 参考 文档https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
        /// </summary>
        /// <param name="services"></param>
        /// <param name="DIMethod"></param>
        public static void InjectionRepositorys(IServiceCollection services, string DIMethod)
        {
            switch (DIMethod)
            {
                case "Scoped":
                    // AddScoped 作用域 方法使用范围内生存期（单个请求的生存期）注册服务。作用域生存期服务 (AddScoped) 以每个客户端请求（连接）一次的方式创建。
                    services.AddScoped(typeof(IRepository<,>), typeof(EfCoreBaseRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(EfCoreBaseRepository<>));
                    break;

                case "Transient":
                    //暂时/瞬态 暂时生存期服务 (AddTransient) 是每次从服务容器进行请求时创建的。 这种生存期适合轻量级、 无状态的服务。
                    services.AddTransient(typeof(IRepository<,>), typeof(EfCoreBaseRepository<,>));
                    services.AddTransient(typeof(IRepository<>), typeof(EfCoreBaseRepository<>));
                    break;

                case "Singleton":
                    //单例 在首次请求它们时进行创建；或者在向容器直接提供实现实例时由开发人员进行创建。 很少用到此方法
                    services.AddSingleton(typeof(IRepository<,>), typeof(EfCoreBaseRepository<,>));
                    services.AddSingleton(typeof(IRepository<>), typeof(EfCoreBaseRepository<>));
                    break;

                default:
                    services.AddScoped(typeof(IRepository<,>), typeof(EfCoreBaseRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(EfCoreBaseRepository<>));
                    break;
            }
        }
    }
}