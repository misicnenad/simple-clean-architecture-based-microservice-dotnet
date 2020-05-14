using System.IO;
using System.Net.Http;

using AccountManager.API;
using AccountManager.API.Configurations;
using AccountManager.Infrastructure.Configurations;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AccountManager.Tests.IntegrationTests
{
    public abstract class TestFixture
    {
        protected readonly IHostBuilder hostBuilder;

        protected IHost host;
        protected HttpClient client;

        protected TestFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Staging.json", optional: true)
                .Build();

            hostBuilder = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutoMapperModule(typeof(InfrastructureModule).Assembly));
                    builder.RegisterModule<InfrastructureModule>();
                })
                .ConfigureWebHost(conf =>
                {
                    conf.UseTestServer();
                    conf.UseStartup<TestStartup>();
                    conf.UseConfiguration(config);

                    // Ignore the TestStartup class assembly as the "entry point" and 
                    // instead point it to this assembly
                    conf.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.FullName);
                });
        }
    }

    internal static class Extensions
    {
        internal static TResult Resolve<TResult>(this IHost host)
        {
            return host.Services.GetAutofacRoot().Resolve<TResult>();
        }
    }
}
