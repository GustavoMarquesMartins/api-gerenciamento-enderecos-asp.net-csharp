using AddressManagement.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using GerenciamentoDeEndereco.Model;

namespace AddressManagement.Model
{
    /// <summary>
    /// Model class representing a User entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

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
        /// Gets or sets the password reset tokens.
        /// </summary>
        public List<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
