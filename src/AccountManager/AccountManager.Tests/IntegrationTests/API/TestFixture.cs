using System.IO;

using AccountManager.API;
using AccountManager.API.Configurations;
using AccountManager.Infrastructure.Configurations;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AccountManager.Tests.IntegrationTests.API
{
    public abstract class TestFixture
    {
        protected readonly IHostBuilder hostBuilder;

        protected TestFixture()
        {
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
