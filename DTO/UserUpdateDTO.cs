using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for updating user details.
    /// </summary>
    public class UserUpdateDTO
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Validates the fields for correct format.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when a field is invalid</exception>
        /// <exception cref="PasswordInvalidaException">Thrown when the password is invalid</exception>
        public void ValidateDate()
        {
            if (Name != null)
            {
                var expectedName = Name + " ";
                string pattern = @"^(([A-Z][a-z]{1,})\s{1}){2,}$";
                if (!Regex.IsMatch(expectedName, pattern))
                    throw new ArgumentException("The full name field is invalid.");
            }

            if (Email != null)
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(Email, pattern))
                    throw new ArgumentException("The email field is invalid.");
            }

            if (Password != null)
            {
                string pattern = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(Password, pattern) || Password.Length < 6)
                    throw new PasswordInvalidaException();
            }
        }
    }

    /// <summary>
    /// Exception thrown when a password does not meet the required criteria.
    /// </summary>
    public class PasswordInvalidaException : Exception
    {
        public PasswordInvalidaException() : base("The password must meet the following criteria:\n" +
                                                  "- Contain one or more special characters.\n" +
                                                  "- Contain at least one uppercase letter.\n" +
                                                  "- Have a minimum length of 6 characters.")
        {
        }
    }
}
