using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using UserManager.Domain;

namespace UserManager.Infrastructure.Configurations
{
    internal static class RequestHandlerRegistrationExtension
    {
        internal static void RegisterRequestHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            var types = GetRequestImplementationTypes(assembly);

            foreach (var type in types)
            {
                var typeInterface = GetRequestInterfaceType(type);

                var returnType = typeInterface.GetGenericArguments()[0];
                var requestWrapperType = typeof(RequestWrapper<,>).MakeGenericType(type, returnType);
                var requestHandlerWrapperType = typeof(RequestHandlerWrapper<,,>).MakeGenericType(requestWrapperType, type, returnType);
                var iRequestHandlerType = typeof(MediatR.IRequestHandler<,>).MakeGenericType(requestWrapperType, returnType);

                builder.RegisterType(requestHandlerWrapperType).As(iRequestHandlerType);
            }
        }

        private static IEnumerable<Type> GetRequestImplementationTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => t.GetInterfaces()
                    .Any(i => (i.IsGenericType
                            && typeof(IRequest<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                        || typeof(IRequest).IsAssignableFrom(i)));
        }

        private static Type GetRequestInterfaceType(Type type)
        {
            return type.GetInterfaces()
                .Where(i => (i.IsGenericType
                        && typeof(IRequest<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                    || typeof(IRequest).IsAssignableFrom(i))
                .Single();
        }
    }
}
