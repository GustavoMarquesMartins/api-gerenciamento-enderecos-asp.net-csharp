using Org.BouncyCastle.Bcpg.OpenPgp;

namespace AddressManagement.DTO
{
    /// <summary>
    /// Data Transfer Object for password reset request code.
    /// </summary>
    public class PasswordRequestCodeDTO
    {
        /// <summary>
        /// Gets or sets the password reset request code.
        /// </summary>
        public string Code { get; set; }
    }
}
