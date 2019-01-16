using Microsoft.Extensions.DependencyInjection;
using MSToolKit.EmailServices.Abstraction;

namespace MSToolKit.EmailServices.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default instance for MSToolKit.EmailServices.Abstraction.IEmailSender to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled email services.
        /// </returns>
        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            return services;
        }
    }
}
