using MimeKit.Cryptography;

namespace GerenciamentoDeEndereco.Infra
{
    /// <summary>
    /// Represents the settings required for email operations.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the URL for the password reset API.
        /// </summary>
        public string PasswordResetApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the email address for SMTP authentication.
        /// </summary>
        public string SmtpEmail { get; set; }

        /// <summary>
        /// Gets or sets the application-specific password for SMTP authentication.
        /// </summary>
        public string SmtpAppPassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSettings"/> class with the specified values.
        /// </summary>
        /// <param name="passwordResetApiUrl">The URL for the password reset API.</param>
        /// <param name="smtpEmail">The email address for SMTP authentication.</param>
        /// <param name="smtpAppPassword">The application-specific password for SMTP authentication.</param>
        public EmailSettings(string passwordResetApiUrl, string smtpEmail, string smtpAppPassword)
        {
            this.PasswordResetApiUrl = passwordResetApiUrl;
            this.SmtpEmail = smtpEmail;
            this.SmtpAppPassword = smtpAppPassword;
        }
    }
}
