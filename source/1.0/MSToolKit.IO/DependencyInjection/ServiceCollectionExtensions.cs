using Microsoft.Extensions.DependencyInjection;
using MSToolKit.IO.Abstraction;

namespace MSToolKit.IO.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default instance for MSToolKit.IO.Abstraction.IFileProcessor to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled file services.
        /// </returns>
        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            services.AddTransient<IFileProcessor, FileProcessor>();

            return services;
        }
    }
}
