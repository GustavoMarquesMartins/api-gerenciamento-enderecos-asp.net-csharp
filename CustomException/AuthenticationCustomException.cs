namespace GerenciamentoDeEndereco.CustomExceptions
{
    /// <summary>
    /// Represents a custom exception related to authentication operations.
    /// </summary>
    public class AuthenticationCustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCustomException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AuthenticationCustomException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Represents an exception thrown when a user is not authenticated.
    /// </summary>
    public class UserNotAuthenticated : AuthenticationCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotAuthenticated"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UserNotAuthenticated(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when an invalid claim identifier is encountered.
    /// </summary>
    public class InvalidClaimIdentifierException : AuthenticationCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidClaimIdentifierException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidClaimIdentifierException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when the user credentials are invalid.
    /// </summary>
    public class InvalidCredentialsException : AuthenticationCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
