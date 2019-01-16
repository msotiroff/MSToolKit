using MSToolKit.Authentication.Abstraction;
using MSToolKit.Authentication.Options;
using MSToolKit.Validation.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MSToolKit.Authentication
{
    /// <summary>
    /// Provides a default instance for MSToolKit.Authentication.Abstraction.IUserValidator.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    internal class UserValidator<TUser> : IUserValidator<TUser> where TUser : AuthenticationUser
    {
        private readonly UserOptions options;

        /// <summary>
        /// Initialize a new instance of MSToolKit.Authentication.UserValidator.
        /// </summary>
        /// <param name="userOptions">
        /// MSToolKit.Authentication.Options.AuthenticationOptions, 
        /// that configures the behavior of the current instance.
        /// </param>
        public UserValidator(UserOptions userOptions)
        {
            this.options = userOptions;
        }

        /// <summary>
        /// Validates the given user against its attributes and 
        /// the AuthenticationOptions, specified in the current instance dependencies.
        /// </summary>
        /// <param name="user">The user to be validated.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing MSToolKit.Authentication.AuthenticationResult of the operation.
        /// </returns>
        public async Task<AuthenticationResult> ValidateAsync(TUser user)
        {
            var authResult = await Task
                .Run(() =>
                {
                    return this.ValidateUser(user);
                });

            return authResult;
        }

        private AuthenticationResult ValidateUser(TUser user)
        {
            var validationResult = user.Validate();
            if (!validationResult.Success)
            {
                return new AuthenticationResult(false, validationResult.Errors.First());
            }

            var notAllowedCharacters = user.Username
                .Where(ch => !this.options.AllowedUserNameCharacters.Contains(ch));

            if (notAllowedCharacters.Any())
            {
                return new AuthenticationResult(
                    false,
                    $"Username cannot contain the following " +
                    $"charaters {string.Join(", ", notAllowedCharacters.Select(ch => $"'{ch}'"))}.");
            }

            return new AuthenticationResult(true);
        }
    }
}
