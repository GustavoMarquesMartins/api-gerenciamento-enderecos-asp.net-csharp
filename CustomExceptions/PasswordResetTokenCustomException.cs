namespace GerenciamentoDeEndereco.CustomExceptions
{
    /// <summary>
    /// Represents a custom exception related to password reset token operations.
    /// </summary>
    public class PasswordReseTokenCustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordReseTokenCustomException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PasswordReseTokenCustomException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when a password reset relationship is not found.
    /// </summary>
    public class PasswordResetRelationshipNotFoundException : PasswordReseTokenCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetRelationshipNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PasswordResetRelationshipNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when a password reset token has expired.
    /// </summary>
    public class TokenExpired : PasswordReseTokenCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenExpired"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TokenExpired(string message) : base(message) { }
    }

}
