namespace GerenciamentoDeEndereco.CustomExceptions
{
    /// <summary>
    /// Represents a custom exception related to user operations.
    /// </summary>
    public class UserCustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCustomException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UserCustomException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when a user is not found.
    /// </summary>
    public class UserNotFound : UserCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFound"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UserNotFound(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when the new password is the same as the old password.
    /// </summary>
    public class PasswordCannotBeSameAsOldException : UserCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCannotBeSameAsOldException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PasswordCannotBeSameAsOldException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when the new password and its confirmation do not match.
    /// </summary>
    public class PasswordsDoNotMatch : UserCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordsDoNotMatch"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PasswordsDoNotMatch(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when an email is already registered.
    /// </summary>
    public class EmailAlreadyRegisteredException : UserCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAlreadyRegisteredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EmailAlreadyRegisteredException(string message) : base(message) { }
    }
}
