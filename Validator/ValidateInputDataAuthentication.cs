namespace GerenciamentoDeEndereco.Validators
{
    /// <summary>
    /// Class for validating input data related to authentication.
    /// </summary>
    public class ValidateInputDataAuthentication
    {
        /// <summary>
        /// Validates the format of the provided code.
        /// </summary>
        /// <param name="code">The code to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the code length is not 6 characters.</exception>
        public static void Code(string code)
        {
            if (code.Length != 6)
                throw new ArgumentException("The code is invalid.");
        }
    }
}
