using Microsoft.Extensions.DependencyInjection;
using MSToolKit.Authentication.Abstraction;
using MSToolKit.Authentication.Options;
using MSToolKit.DataAccess.Abstraction;
using System;
using System.Collections.Generic;

namespace MSToolKit.Authentication.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static HashSet<Type> registeredUserStores = new HashSet<Type>();
        
        /// <summary>
        /// Adds authentication services with specified(or default, if missing) options to the service provider.
        /// </summary>
        /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <param name="options">Instance of MSToolKit.Authentication.Options.AuthenticationOptions(optional parameter).</param>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled authentication services.
        /// </returns>
        public static IServiceCollection AddAuthenticationServices<TUser>(
            this IServiceCollection services, Action<AuthenticationOptions> options)
            where TUser : AuthenticationUser
        {
            var authOptions = new AuthenticationOptions();
            options?.Invoke(authOptions);

            services.AddSingleton(authOptions);
            services.AddSingleton(authOptions.Password);
            services.AddSingleton(authOptions.User);
            services.AddSingleton(authOptions.SignIn);
            services.AddTransient<ISignInManager<TUser>, SignInManager<TUser>>();
            services.AddTransient<IUserManager<TUser>, UserManager<TUser>>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IUserValidator<TUser>, UserValidator<TUser>>();

            return services;
        }

        /// <summary>
        /// Adds a specified user store to the service provider. Only one user store per user type is allowed.
        /// </summary>
        /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
        /// <typeparam name="TStore">The type encapsulating a user store.</typeparam>
        /// <param name="services">The service collection, that will service provider be built from.</param>
        /// <exception cref="InvalidOperationException">
        /// System.InvalidOperationException will be thrown, if method's already invoked with the same type of TUser.
        /// </exception>
        /// <returns>
        /// The same instane of Microsoft.Extensions.DependencyInjection.IServiceCollection with filled user store services.
        /// </returns>
        public static IServiceCollection AddUserStore<TUser, TStore>(this IServiceCollection services)
            where TStore : IDbContext<TUser, string> where TUser : AuthenticationUser
        {
            if (registeredUserStores.Contains(typeof(TUser)))
            {
                throw new InvalidOperationException(
                    $"You have already added a user store for type: {typeof(TUser).FullName}.");
            }

            services.AddTransient<IUserStore<TUser>>(sp =>
            {
                var store = sp.GetRequiredService<TStore>();
                return new UserStore<TUser>(store);
            });

            registeredUserStores.Add(typeof(TUser));
            return services;
        }

    }
}
