namespace GerenciamentoDeEndereco.CustomExceptions
{
    /// <summary>
    /// Represents a custom exception related to address operations.
    /// </summary>
    public class AddressCustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressCustomException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AddressCustomException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an exception thrown when an address is not found.
    /// </summary>
    public class AddressNotFound : AddressCustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressNotFound"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AddressNotFound(string message) : base(message) { }
    }

}
