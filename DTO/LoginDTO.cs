using GerenciamentoDeEndereco.Validators;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Validates the input data for the login.
        /// This method ensures that the email and password adhere to specified validation rules.
        /// </summary>
        public void ValidateData()
        {
            // Validate the email format using custom validation logic
            ValidateInputDataUser.Email(Email);

            // Validate the password strength and format using custom validation logic
            ValidateInputDataUser.Password(Password);
        }
    }
}
