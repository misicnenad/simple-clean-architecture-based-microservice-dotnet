using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MediatR;
using UserManager.Domain.Commands;

namespace UserManager.Infrastructure.Configurations
{
    internal static class RequestHandlerRegistrationExtension
    {
        internal static void RegisterRequestHandlers(this ContainerBuilder builder)
        {
            var types = GetRequestImplementationTypes();

            foreach (var type in types)
            {
                var typeInterface = GetRequestInterfaceType(type);

                var returnType = typeInterface.GetGenericArguments()[0];
                var requestWrapperType = typeof(RequestWrapper<,>).MakeGenericType(type, returnType);
                var requestHandlerWrapperType = typeof(RequestHandlerWrapper<,,>).MakeGenericType(requestWrapperType, type, returnType);
                var iRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(requestWrapperType, returnType);

                builder.RegisterType(requestHandlerWrapperType).As(iRequestHandlerType);
            }
        }

        private static IEnumerable<Type> GetRequestImplementationTypes()
        {
            return typeof(Command).Assembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => t.GetInterfaces()
                    .Any(i => (i.IsGenericType
                            && typeof(Domain.IRequest<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                        || typeof(Domain.IRequest).IsAssignableFrom(i)));
        }

        private static Type GetRequestInterfaceType(Type type)
        {
            return type.GetInterfaces()
                .Where(i => (i.IsGenericType
                        && typeof(Domain.IRequest<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                    || typeof(Domain.IRequest).IsAssignableFrom(i))
                .Single();
        }
    }
}
