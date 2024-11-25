using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
