using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressManagement.Model;

namespace GerenciamentoDeEndereco.Model
{
    /// <summary>
    /// Represents a password reset token for a user.
    /// </summary>
    public class PasswordResetToken
    {
        /// <summary>
        /// Gets or sets the unique ID for the token.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email associated with the token.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the generated token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the verification code token.
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the token.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the token.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the token.
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
