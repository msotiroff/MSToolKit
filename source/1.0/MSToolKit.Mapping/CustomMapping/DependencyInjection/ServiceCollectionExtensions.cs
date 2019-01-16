using Microsoft.Extensions.DependencyInjection;
using MSToolKit.Mapping.CustomMapping.Abstraction;
using System;

namespace MSToolKit.Mapping.CustomMapping.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add a default implementation of MSToolKit.Mapping.CustomMapping.Abstraction.ICustomMapper 
        /// that provides a functionality for default mapping between objects.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddCustomMapper(this IServiceCollection services)
        {
            services.AddTransient<ICustomMapper, CustomMapper>();
            services.AddTransient(typeof(Mapping<,>), typeof(Mapping<,>));

            return services;
        }

        /// <summary>
        /// Add a default mapping between two type of objects.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <param name="convertion">
        /// A delegate that accepts a source object and returns transformed destination object.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddCustomMapping<TSource, TDestination>(
            this IServiceCollection services,
            Func<TSource, TDestination> convertion)
        {
            services.AddTransient(sp => new Mapping<TSource, TDestination>(convertion));

            return services;
        }

    }
}
