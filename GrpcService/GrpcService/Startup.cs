using GrpcService.Services;
using Hy.FantasyGrpcEFCore.DbDao;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NConsul.AspNetCore;
using System;
using System.Configuration;

namespace GrpcService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //log4net
            // repository = LogManager.CreateRepository("NETCoreRepository");
            //指定配置文件
            // XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            //调用工厂模式进行依赖注入
            InitializationFactory.Injection(services, Configuration);

            //services.AddConsul("http://localhost:8500")
            //    .AddGRPCHealthCheck("localhost:5001")
            //    .RegisterService("stonetest", "localhost", 5001, new[] { "xc/grpc/test" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<LuDogService>();

                endpoints.MapGrpcService<LuPigService>();
                endpoints.MapGrpcService<LuCatService>();
                endpoints.MapGrpcService<HealthCheckService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}