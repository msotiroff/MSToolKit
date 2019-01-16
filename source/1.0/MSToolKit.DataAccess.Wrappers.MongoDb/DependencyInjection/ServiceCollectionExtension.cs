using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MSToolKit.DataAccess.Abstraction;
using MSToolKit.DataAccess.Wrappers.MongoDb.Abstraction;
using MSToolKit.Validation.Extensions;
using System;

namespace MSToolKit.DataAccess.Wrappers.MongoDb.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds a default instance of MSToolKit.Wrappers.MongoDb.Abstraction.IMongoDbWrapper
        /// for a specified entity with specified primary key.
        /// </summary>
        /// <typeparam name="TEntity">The type encapsulating the entity.</typeparam>
        /// <typeparam name="TKey">The primary key's type for the given entity.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="tableName">The name of the table, that contains the entities of the given type.</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled MongoDbWrapper services.
        /// </returns>
        public static IServiceCollection AddMongoDbWrapper<TEntity, TKey>(this IServiceCollection services, string tableName)
             where TEntity : IEntity<TKey>
        {
            services.AddTransient<IMongoDbWrapper<TEntity, TKey>>(
                sp => new MongoDbWrapper<TEntity, TKey>(
                    sp.GetRequiredService<IMongoDatabase>(), tableName));

            return services;
        }

        /// <summary>
        /// Adds a default instance for MongoDB.Driver.IMongoDatabase with specified options.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="mongoDbOptions">
        /// MSToolKit.Wrappers.MongoDb.MongoDbOptions, that configure the IMongoDatabase instance.
        /// </param>
        /// <exception cref="ArgumentException">
        /// System.ArgumentException will be thrown, if some of the required options are not specified or invalid.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled mongo db services.
        /// </returns>
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, Action<MongoDbOptions> mongoDbOptions)
        {
            var options = new MongoDbOptions();
            mongoDbOptions?.Invoke(options);

            var optionsValidationResult = options.Validate();
            if (!optionsValidationResult.Success)
            {
                var errorMsg = "MongoDbOptions validation failed. Errors: "
                    + string.Join(" ", optionsValidationResult.Errors);

                throw new ArgumentException(errorMsg);
            }

            services.AddTransient<IMongoDatabase>(sp =>
            {
                var client = new MongoClient(options.ConnectionString);
                return client.GetDatabase(options.DatabaseName);
            });

            return services;
        }
    }
}
