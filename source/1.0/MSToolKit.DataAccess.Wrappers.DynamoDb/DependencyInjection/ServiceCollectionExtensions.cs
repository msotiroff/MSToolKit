using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using MSToolKit.DataAccess.Abstraction;
using MSToolKit.DataAccess.Wrappers.DynamoDb.Abstraction;
using MSToolKit.Validation.Extensions;
using System;

namespace MSToolKit.DataAccess.Wrappers.DynamoDb.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default instance for MSToolKit.Wrappers.DynamoDb.Abstraction.IDynamoDbWrapper with specified options.
        /// </summary>
        /// <typeparam name="TEntity">The type encapsulating the entity.</typeparam>
        /// <typeparam name="TKey">The primary key's type for the given entity.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="tableOptions">
        /// MSToolKit.Wrappers.DynamoDb.DynamoDbTable options, that confifure the DynamoDbWrapper instance.
        /// </param>
        /// <exception cref="ArgumentException">
        /// System.ArgumentException will be thrown, if some of the required options are not specified or invalid.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled DynamoDbWrapper services.
        /// </returns>
        public static IServiceCollection AddDynamoDbWrapper<TEntity, TKey>(
            this IServiceCollection services,
            Action<DynamoDbTableOptions<TEntity, TKey>> tableOptions) where TEntity : IEntity<TKey>
        {
            var tableOpt = new DynamoDbTableOptions<TEntity, TKey>();
            tableOptions?.Invoke(tableOpt);

            var tableValidationResult = tableOpt.Validate();
            if (!tableValidationResult.Success)
            {
                var errorMsg = "DynamoDbTable options validation failed. Errors: "
                    + string.Join(" ", tableValidationResult.Errors);

                throw new ArgumentException(errorMsg);
            }

            services.AddTransient<IDynamoDbWrapper<TEntity, TKey>>(
                sp => new DynamoDbWrapper<TEntity, TKey>(
                    sp.GetRequiredService<IAmazonDynamoDB>(), tableOpt.TableName));

            services.AddTransient<IDbContext<TEntity, TKey>>(
                sp => sp.GetRequiredService<IDynamoDbWrapper<TEntity, TKey>>());

            return services;
        }

        /// <summary>
        /// Adds a default instance for Amazon.DynamoDBv2.DataModel.IDynamoDBContext with specified options.
        /// </summary>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="dynamoDbOptions">
        /// MSToolKit.Wrappers.DynamoDb.DynamoDbOptions, that configure the default instance for IDynanoDbContext.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with added DynamoDbContext services.
        /// </returns>
        public static IServiceCollection AddDynamoDbContext(
            this IServiceCollection services, Action<DynamoDbOptions> dynamoDbOptions = null)
        {
            var options = new DynamoDbOptions();
            dynamoDbOptions?.Invoke(options);

            if (options.LocalMode == true)
            {
                var clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = options.ServiceUrl ?? null
                };

                if (clientConfig.ServiceURL == null)
                {
                    clientConfig.RegionEndpoint = options.RegionEndpoint;
                }

                services.AddTransient<IAmazonDynamoDB>(sp =>
                {
                    if (options.AccessKeyId != null && options.SecretAccessKey != null)
                    {
                        return new AmazonDynamoDBClient(
                            options.AccessKeyId,
                            options.SecretAccessKey,
                            clientConfig);
                    }

                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddTransient<IDynamoDBContext>(
                sp => new DynamoDBContext(sp.GetService<IAmazonDynamoDB>()));

            return services;
        }
    }
}
