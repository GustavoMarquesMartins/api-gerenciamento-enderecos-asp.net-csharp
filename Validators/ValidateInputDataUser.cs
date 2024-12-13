using GerenciamentoDeEndereco.DTO;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.Validators
{
    /// <summary>
    /// Service for validating input data such as name, email, and password.
    /// </summary>
    public class ValidateInputDataUser
    {
        /// <summary>
        /// Validates the input name to ensure it follows the expected pattern.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <exception cref="Exception">Thrown when the name is invalid.</exception>
        public static void Name(string name)
        {
            // Append a space to the name to match the expected pattern
            var expectedName = name + " ";
            // Define the regex pattern for a valid name (two or more capitalized words)
            string pattern = @"^(([A-Z][a-z]{1,})\s{1}){2,}$";
            // Validate the name against the pattern
            if (!Regex.IsMatch(expectedName, pattern))
                throw new Exception("The full name field is invalid.");
        }

        /// <summary>
        /// Validates the input email to ensure it follows the standard email format.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <exception cref="Exception">Thrown when the email is invalid.</exception>
        public static void Email(string email)
        {
            // Define the regex pattern for a valid email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            // Validate the email against the pattern
            if (!Regex.IsMatch(email, pattern))
                throw new Exception("The email field is invalid.");
        }

        /// <summary>
        /// Validates the input password to ensure it meets security requirements.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="Exception">Thrown when the password is invalid.</exception>
        public static void Password(string password)
        {
            // Define the regex pattern for a valid password (at least one uppercase letter and one special character)
            string pattern = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
            // Validate the password against the pattern and ensure it is at least 6 characters long
            if (!Regex.IsMatch(password, pattern) || password.Length < 6)
                throw new Exception("The password is invalid.");
        }
    }
}
