using Microsoft.Extensions.DependencyInjection;
using MSToolKit.IO.Excel.Abstraction;

namespace MSToolKit.IO.Excel.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default instance for MSToolKit.IO.Excel.Abstraction.IExcelGenerator to the service provider.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled excel services.
        /// </returns>
        public static IServiceCollection AddExcelServices(this IServiceCollection services)
        {
            services.AddTransient<IExcelBuilder, ExcelBuilder>();

            return services;
        }
    }
}
