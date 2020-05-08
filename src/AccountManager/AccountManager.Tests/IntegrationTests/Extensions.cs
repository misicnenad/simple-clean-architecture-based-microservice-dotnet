using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountManager.Tests.IntegrationTests
{
    public static class Extensions
    {
        public static TResult Resolve<TResult>(this IHost host)
        {
            return host.Services.GetAutofacRoot().Resolve<TResult>();
        }
    }
}
