using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    [AllowAnonymous]
    /// <summary>
    /// Data Transfer Object for creating a new user.
    /// </summary>
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
    }
}
