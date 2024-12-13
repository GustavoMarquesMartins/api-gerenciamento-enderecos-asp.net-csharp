using GerenciamentoDeEndereco.Validators;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for requesting a password change.
    /// </summary>
    public class PasswordChangeRequestDTO
    {
        /// <summary>
        /// Gets or sets the token used for password reset.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        public string NewPasswordConfirm { get; set; }

        /// <summary>
        /// Validates the input data for password change.
        /// </summary>
        /// <exception cref="Exception">Thrown when the new password and confirmation do not match or the password does not meet the requirements.</exception>
        public void ValidateData()
        {
            // Check if the new password and confirmation are the same
            if (NewPassword != NewPasswordConfirm)
                throw new Exception("Passwords do not match");

            // Validate the new password using the ValidateInputDataService
            ValidateInputDataUser.Password(NewPassword);
            ValidateInputDataUser.Password(NewPasswordConfirm);
        }
    }
}
