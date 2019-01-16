using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.DependencyInjection;
using MSToolKit.IO.AmazonFileManagment;
using MSToolKit.IO.AmazonFileManagment.Abstraction;

namespace MSToolKit.IO.AmazonFileManagement.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Amazon S3 servicesas well as default implementation 
        /// of MSToolKit.IO.AmazonFileManagment.Abstraction.IAmazonS3FileManager.
        /// </summary>
        /// <param name="services">
        /// The service collection, that will service provider be built from.
        /// </param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddAmazonS3FileManagement(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IAmazonS3FileManager, AmazonS3FileManager>();
            services.AddTransient<ITransferUtility>(
                sp => new TransferUtility(sp.GetRequiredService<IAmazonS3>()));

            return services;
        }

    }
}
