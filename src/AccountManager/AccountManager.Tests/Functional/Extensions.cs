using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

namespace AccountManager.Tests.Functional
{
    internal static class Extensions
    {
        internal static TResult Resolve<TResult>(this IHost host)
        {
            return host.Services.GetAutofacRoot().Resolve<TResult>();
        }
    }
}
