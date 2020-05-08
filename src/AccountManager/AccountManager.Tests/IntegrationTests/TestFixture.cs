using System.IO;
using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using AccountManager.API;
using AccountManager.API.Configurations;
using AccountManager.Domain.Interfaces;
using AccountManager.Infrastructure.Configurations;
using System;

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
                    builder.RegisterModule<MediatRModule>();

                    // Mock the event service so no events are sent to EventHub
                    var mockHttpClient = new Mock<IHttpClientFactory>().Object;
                    builder.Register(c => mockHttpClient).As<IHttpClientFactory>();
                })
                .ConfigureWebHost(conf =>
                {
                    conf.UseTestServer();
                    conf.UseStartup<Startup>();
                    conf.UseConfiguration(config);
                });
        }
    }
}
