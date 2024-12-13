using GerenciamentoDeEndereco.Validators;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "The full name field cannot be blank")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Required(ErrorMessage = "The email field cannot be blank")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required(ErrorMessage = "The password field cannot be blank")]
        [MinLength(6, ErrorMessage = "The password must have a minimum length of 6 characters")]
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
