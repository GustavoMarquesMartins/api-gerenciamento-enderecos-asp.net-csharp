using System.Net.Mail;
using System.Net;
using AddressManagement.Model;
using AddressManagement.Infra;
using Microsoft.EntityFrameworkCore;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Service;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.CustomExceptions;
using GerenciamentoDeEndereco.Validators;
using System.Drawing;

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
        /// Initializes a new instance of the <see cref="PasswordResetService"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="emailSettings">The email settings for SMTP configuration.</param>
        /// <param name="userService">The user service for user operations.</param>
        public PasswordResetService(UserDbContext db, EmailSettings emailSettings, UserService userService)
        {
            _db = db;
            _emailSettings = emailSettings;
            _userService = userService;
        }

        /// <summary>
        /// Initializes the process of sending a password reset email.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        public async Task InitializeEmailDispatch(string email)
        {
            var passwordResetToken = await CreatePasswordResetTokenAsync(email);
            await SendEmailAsync(email, passwordResetToken.VerificationCode);
        }

        /// <summary>
        /// Retrieves a password reset token by its token string.
        /// </summary>
        /// <param name="token">The token string to look up.</param>
        /// <returns>The password reset token entity.</returns>
        /// <exception cref="PasswordResetRelationshipNotFoundException">
        /// Thrown when the token relationship is not found.
        /// </exception>
        private async Task<PasswordResetToken> GetPasswordResetTokenByToken(string token)
        {
            var result = await _db.PasswordResetTokens
                .Include(prt => prt.User)
                .FirstOrDefaultAsync(prt => prt.Token == token);

            if (result == null)
                throw new PasswordResetRelationshipNotFoundException("Relationship with the provided email not found.");

            return result;
        }

        /// <summary>
        /// Sends a password reset email with the provided verification code.
        /// </summary>
        /// <param name="addressee">The recipient email address.</param>
        /// <param name="verificationCode">The verification code to include in the email.</param>
        public async Task SendEmailAsync(string addressee, string verificationCode)
        {
            ValidateInputDataPasswordResetToken.Addressee(addressee);
            ValidateInputDataPasswordResetToken.VerificationCode(verificationCode);

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                var smtpEmail = _emailSettings.SmtpEmail;
                var smtpPassword = _emailSettings.SmtpAppPassword;

                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
                smtpClient.EnableSsl = true;

                var templatePath = "./Source/EmailBody.html";
                var htmlContent = await File.ReadAllTextAsync(templatePath);
                htmlContent = htmlContent.Replace("{VerificationCode}", verificationCode);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpEmail),
                    Subject = "Password Reset",
                    Body = htmlContent,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(addressee);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        /// <summary>
        /// Generates a unique 6-digit verification code.
        /// </summary>
        /// <returns>A unique verification code.</returns>
        private async Task<string> GenerateVerificationCode()
        {
            Random random = new Random();
            string verificationCode;

            do
            {
                verificationCode = random.Next(100000, 1000000).ToString();
            } while (!await IsVerificationCodeUnique(verificationCode));

            return verificationCode;
        }

        /// <summary>
        /// Checks whether a verification code is unique.
        /// </summary>
        /// <param name="verificationCode">The verification code to check.</param>
        /// <returns>True if the code is unique; otherwise, false.</returns>
        private async Task<bool> IsVerificationCodeUnique(string verificationCode)
        {
            var result = await _db.PasswordResetTokens
                .FirstOrDefaultAsync(c => c.VerificationCode == verificationCode);
            return result == null;
        }

        /// <summary>
        /// Creates a new password reset token for a given email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The generated password reset token.</returns>
        public async Task<PasswordResetToken> CreatePasswordResetTokenAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                throw new UserNotFound("User with the provided email was not found in the system.");

            var token = TokenGenerator.GenerateToken();
            var expirationDate = DateTime.Now.AddMinutes(10);
            var verificationCode = await GenerateVerificationCode();

            var passwordResetToken = new PasswordResetToken
            {
                Expiration = expirationDate,
                Email = user.Email,
                Token = token,
                VerificationCode = verificationCode,
                UserId = user.Id
            };

            if (user.PasswordResetTokens == null)
                user.PasswordResetTokens = new List<PasswordResetToken>();

            user.PasswordResetTokens.Add(passwordResetToken);

            return await SavePasswordResetTokenAsync(passwordResetToken);
        }

        /// <summary>
        /// Saves a password reset token to the database.
        /// </summary>
        /// <param name="passwordResetToken">The token to save.</param>
        /// <returns>The saved password reset token.</returns>
        public async Task<PasswordResetToken> SavePasswordResetTokenAsync(PasswordResetToken passwordResetToken)
        {
            var result = await _db.PasswordResetTokens.AddAsync(passwordResetToken);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Verifies the validity of a token.
        /// </summary>
        /// <param name="token">The token to verify.</param>
        /// <returns>The user associated with the token.</returns>
        public async Task<User> VerifyTokenValidityAsync(string token)
        {
            var result = await GetPasswordResetTokenByToken(token);
            VerifyExpiration(result.Expiration);
            return result.User;
        }

        /// <summary>
        /// Retrieves a password reset token by verification code.
        /// </summary>
        /// <param name="code">The verification code.</param>
        /// <returns>The password reset token associated with the code.</returns>
        public async Task<PasswordResetToken> GetPasswordResetTokenByCode(string code)
        {
            var result = await _db.PasswordResetTokens
                .Include(prt => prt.User)
                .FirstOrDefaultAsync(prt => prt.VerificationCode == code);

            if (result == null)
                throw new PasswordResetRelationshipNotFoundException("Relationship with the provided code not found.");

            return result;
        }

        /// <summary>
        /// Verifies the validity of a verification code.
        /// </summary>
        /// <param name="code">The verification code.</param>
        /// <returns>The associated token.</returns>
        public async Task<string> VerifyCodeValidityAsync(string code)
        {
            var result = await GetPasswordResetTokenByCode(code);
            VerifyExpiration(result.Expiration);
            return result.Token;
        }

        /// <summary>
        /// Checks whether a token has expired.
        /// </summary>
        /// <param name="expiration">The expiration date of the token.</param>
        public void VerifyExpiration(DateTime expiration)
        {
            if (expiration < DateTime.Now)
                throw new TokenExpired("Token expired.");
        }

        /// <summary>
        /// Updates the user's password using the provided token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newPasswordConfirm">The confirmation of the new password.</param>
        public async Task UpdateUserPasswordAsync(string token, string newPassword, string newPasswordConfirm)
        {
            ValidateInputDataUser.IsPasswordEqual(newPassword, newPasswordConfirm);
            var user = await VerifyTokenValidityAsync(token);
            ValidateInputDataUser.IsNewPasswordDifferenIsPasswordEqualtFromOld(newPassword, user.Password);

            user.Password = newPassword;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
