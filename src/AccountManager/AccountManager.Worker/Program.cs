using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using AccountManager.Infrastructure.Configurations;
using AccountManager.Worker.Configurations;
using AccountManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new WorkerModule());
                    builder.RegisterModule(new InfrastructureModule());
                    builder.RegisterModule(new AutoMapperModule(typeof(InfrastructureModule).Assembly));
                })
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHttpClient();

                    // DbContext is registered here because Autofac doesn't support EF migrations.
                    // Also mocking the DB with an in-memory DB for tests is easier
                    services.AddDbContext<AccountManagerDbContext>(opt =>
                        opt.UseSqlServer(Configuration.GetConnectionString("AccountManagerDbConnectionString")));
                });

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();
    }
}
