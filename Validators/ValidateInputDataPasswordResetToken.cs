using GerenciamentoDeEndereco.CustomExceptions;
using GerenciamentoDeEndereco.Model;

namespace GerenciamentoDeEndereco.Validators
{
    /// <summary>
    /// Provides validation methods for password reset token input data.
    /// </summary>
    public class ValidateInputDataPasswordResetToken
    {
        /// <summary>
        /// Validates the email address (addressee) for password reset.
        /// </summary>
        /// <param name="addressee">The email address to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the email address is null or empty.</exception>
        public static void Addressee(string addressee)
        {
            if (string.IsNullOrEmpty(addressee))
            {
                throw new ArgumentException("Email address cannot be empty.");
            }
        }

        /// <summary>
        /// Validates the verification code for password reset.
        /// </summary>
        /// <param name="code">The verification code to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the verification code is null or empty.</exception>
        public static void VerificationCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Verification code cannot be empty.");
            }
        }
    }
}
