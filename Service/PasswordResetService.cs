using System.Net.Mail;
using System.Net;
using AddressManagement.Model;
using AddressManagement.Infra;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Service;
using GerenciamentoDeEndereco.Model;

namespace AddressManagement.Service
{
    /// <summary>
    /// Service for handling password reset functionality.
    /// </summary>
    public class PasswordResetService
    {
        private readonly UserDbContext _db;
        private readonly EmailSettings _emailSettings;
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the PasswordResetService class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="emailSettings">The email settings.</param>
        /// <param name="userService">The user service.</param>
        public PasswordResetService(UserDbContext db, EmailSettings emailSettings, UserService userService)
        {
            _db = db;
            _emailSettings = emailSettings;
            _userService = userService;
        }

        /// <summary>
        /// Initializes the process of sending a password reset email.
        /// </summary>
        /// <param name="email">The email address to which the password reset email will be sent.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InitializeEmailDispatch(string email)
        {
            // Create a password reset token for the given email address
            var passwordResetToken = await CreatePasswordResetTokenAsync(email);

            // Send the password reset email with the generated token
            await SendEmailAsync(email, passwordResetToken.VerificationCode);
        }

        /// <summary>
        /// Retrieves a password reset token by the given token string.
        /// </summary>
        /// <param name="token">The token string to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token.</returns>
        /// <exception cref="Exception">Thrown when the token is not found.</exception>
        private async Task<PasswordResetToken> GetPasswordResetTokenByToken(string token)
        {
            var result = await _db.PasswordResetTokens
                .Include(prt => prt.User) // Eager loading the User entity
                .FirstOrDefaultAsync(prt => prt.Token == token);

            if (result == null) throw new Exception("Relationship with the provided email not found.");
            return result; // Return the result
        }

        /// <summary>
        /// Sends a password reset email to the specified addressee.
        /// </summary>
        /// <param name="addressee">The recipient email address.</param>
        /// <param name="verificationCode">The password reset verification code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the addressee or verification code is empty.</exception>
        public async Task SendEmailAsync(string addressee, string verificationCode)
        {
            if (string.IsNullOrWhiteSpace(addressee))
                throw new ArgumentException("Email address cannot be empty.", nameof(addressee));
            if (string.IsNullOrWhiteSpace(verificationCode))
                throw new ArgumentException("Verification code cannot be empty.", nameof(verificationCode));

            try
            {
                // Configure the SMTP client using Google's SMTP server
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    var smtpEmail = _emailSettings.SmtpEmail;
                    var smtpPassword = _emailSettings.SmtpAppPassword;

                    smtpClient.Port = 587; // SMTP server port
                    smtpClient.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
                    smtpClient.EnableSsl = true; // Enable SSL

                    // Read the HTML email template
                    var templatePath = "./Source/EmailBody.html";
                    var htmlContent = await File.ReadAllTextAsync(templatePath); // Asynchronous file read operation
                    htmlContent = htmlContent.Replace("{VerificationCode}", verificationCode);

                    // Create the email message
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpEmail),
                        Subject = "Password Reset",
                        Body = htmlContent,
                        IsBodyHtml = true, // Set to true if the email body contains HTML
                    };

                    mailMessage.To.Add(addressee);

                    // Send the email asynchronously
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Generates a verification code for password reset.
        /// </summary>
        /// <returns>A randomly generated verification code as a string.</returns>
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            string verificationCode = random.Next(100000, 1000000).ToString();
            return verificationCode;
        }

        /// <summary>
        /// Creates a password reset token for the specified user.
        /// </summary>
        /// <param name="email">The email address of the user for whom the token is created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created password reset token.</returns>
        public async Task<PasswordResetToken> CreatePasswordResetTokenAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            var token = TokenGenerator.GenerateToken();
            var expirationDate = DateTime.Now.AddMinutes(10);
            var verificationCode = GenerateVerificationCode();

            var passwordResetToken = new PasswordResetToken
            {
                Expiration = expirationDate,
                Email = user.Email,
                Token = token,
                VerificationCode = verificationCode,
                UserId = user.Id
            };

            if (user.PasswordResetTokens == null) user.PasswordResetTokens = new List<PasswordResetToken>();
            user.PasswordResetTokens.Add(passwordResetToken);

            return await SavePasswordResetTokenAsync(passwordResetToken);
        }

        /// <summary>
        /// Saves the password reset token to the database.
        /// </summary>
        /// <param name="passwordResetToken">The password reset token to save.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the saved password reset token.</returns>
        public async Task<PasswordResetToken> SavePasswordResetTokenAsync(PasswordResetToken passwordResetToken)
        {
            var result = await _db.PasswordResetTokens.AddAsync(passwordResetToken);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Verifies the validity of the specified token.
        /// </summary>
        /// <param name="token">The token to verify.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the token.</returns>
        public async Task<User> VerifyTokenValidityAsync(string token)
        {
            var result = await GetPasswordResetTokenByToken(token);

            VerifyExpiration(result.Expiration);
            return result.User;
        }

        /// <summary>
        /// Retrieves a password reset token by the given verification code.
        /// </summary>
        /// <param name="code">The verification code to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token.</returns>
        /// <exception cref="Exception">Thrown when the verification code is not found.</exception>
        public async Task<PasswordResetToken> GetPasswordResetTokenByCode(string code)
        {
            var result = await _db.PasswordResetTokens
                .Include(prt => prt.User)
                .FirstOrDefaultAsync(prt => prt.VerificationCode == code);

            if (result == null) throw new Exception("Relationship with the provided code not found.");
            return result;
        }

        /// <summary>
        /// Verifies the validity of the specified verification code.
        /// </summary>
        /// <param name="code">The verification code to verify.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token associated with the verification code.</returns>
        public async Task<string> VerifyCodeValidityAsync(string code)
        {
            var result = await GetPasswordResetTokenByCode(code);

            VerifyExpiration(result.Expiration);

            return result.Token;
        }

        /// <summary>
        /// Checks if the token has expired.
        /// </summary>
        /// <param name="expiration">The expiration date of the token.</param>
        /// <exception cref="InvalidOperationException">Thrown if the token has expired.</exception>
        public void VerifyExpiration(DateTime expiration)
        {
            if (expiration < DateTime.Now)
                throw new InvalidOperationException("Token expired.");
        }

        /// <summary>
        /// Updates the user's password using the specified token and new password.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newPasswordConfirm">The new password confirmation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the new password and confirmation password do not match.</exception>
        /// <exception cref="Exception">Thrown when the new password is the same as the old password.</exception>
        public async Task UpdateUserPasswordAsync(string token, string newPassword, string newPasswordConfirm)
        {
            if (newPassword != newPasswordConfirm)
                throw new ArgumentException("The new password and confirmation password do not match.");

            var user = await VerifyTokenValidityAsync(token);
            if (user.Password == newPassword)
                throw new Exception("The new password must be different from the old password.");

            user.Password = newPassword;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
