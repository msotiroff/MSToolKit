namespace MSToolKit.Authentication.Abstraction
{
    /// <summary>
    /// Provides an abstraction for validating passwords.
    /// </summary>
    public interface IPasswordValidator
    {
        /// <summary>
        /// Validates the given password.
        /// </summary>
        /// <param name="password">The password in plain text.</param>
        /// <returns>MSToolKit.Authentication.AuthenticationResult</returns>
        AuthenticationResult Validate(string password);
    }
}