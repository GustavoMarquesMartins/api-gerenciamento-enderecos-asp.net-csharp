using GerenciamentoDeEndereco.Validators;
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
        /// Validates the fields for correct format using custom validators.
        /// This method ensures that only non-null fields are validated.
        /// </summary>
        /// <exception cref="Exception">Thrown when a field is invalid.</exception>
        public void ValidateData()
        {
            // Validate the user name using custom validation logic if provided
            if (Name != null)
            {
                ValidateInputDataUser.Name(Name);
            }

            // Validate the email format using custom validation logic if provided
            if (Email != null)
            {
                ValidateInputDataUser.Email(Email);
            }

            // Validate the password strength and format using custom validation logic if provided
            if (Password != null)
            {
                ValidateInputDataUser.Password(Password);
            }
        }
    }
}
