using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace MSToolKit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services that match the following name convention: 
        /// [Interface name = I{serviceName}, Implementation name = {serviceName}] to the service provider.
        /// </summary>
        /// <param name="assembly">The assembly, that services are located.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddConvetionallyNamedServices(
            this IServiceCollection services,
            Assembly assembly)
        {
            var ass = Assembly.GetExecutingAssembly().FullName;
            /// Prevents adding services for MSToolKit.dll
            if (assembly.FullName == Assembly.GetExecutingAssembly().FullName)
            {
                return services;
            }

            assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && !t.IsGenericType
                    && t.GetInterfaces()
                        .Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => services.AddTransient(s.Interface, s.Implementation));

            return services;
        }
    }
}
