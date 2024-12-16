using GerenciamentoDeEndereco.Validators;
using Microsoft.AspNetCore.Authorization;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for creating a new user.
    /// </summary>
    [AllowAnonymous]
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Validates the input data for the new user.
        /// This method ensures that the name, email, and password adhere to specified validation rules.
        /// </summary>
        public void ValidateData()
        {
            // Validate the user name using custom validation logic
            ValidateInputDataUser.Name(Name);

            // Validate the email format using custom validation logic
            ValidateInputDataUser.Email(Email);

            // Validate the password strength and format using custom validation logic
            ValidateInputDataUser.Password(Password);
        }
    }
}
