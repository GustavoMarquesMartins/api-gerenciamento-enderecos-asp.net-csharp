using System.ComponentModel.DataAnnotations;
using GerenciamentoDeEndereco.Validators;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for password reset request.
    /// </summary>
    public class PasswordResetRequestDTO
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Required(ErrorMessage = "The email field is required.")]
        public string Email { get; set; }

        /// <summary>
        /// Validates the input data for the password reset request.
        /// This method ensures that the email adheres to specified validation rules.
        /// </summary>
        public void ValidateData()
        {
            // Validate the email format using custom validation logic
            ValidateInputDataUser.Email(Email);
        }
    }
}
