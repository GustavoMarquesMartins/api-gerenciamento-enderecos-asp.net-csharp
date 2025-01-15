using AddressManagement.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Security.Generator;
using System.Text;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;

namespace AddressManagement.Model
{
    /// <summary>
    /// Model class representing a User entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique ID for the token.
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

        public string Salt { get; set; }

        public string GenerateSalt()
        {
            using(var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16]; // 16 bytes for the salt
                rng.GetBytes(saltBytes); // Fill the array with random values
                Salt = Convert.ToBase64String(saltBytes); // Convert the salt to a Base64 string
                return Salt;
            }
        }

        public string GeneratePasswordHashWithSalt(string password, string salt)
        {
                string passwordWithSalt =  password + salt;

                // Hash the password with the salt
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                    byte[] hashBytes = SHA256.HashData(passwordBytes);

                    StringBuilder hashStringBuilder = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        hashStringBuilder.Append(b.ToString("x2")); // Convert each byte to a hexadecimal string
                    }

                    return hashStringBuilder.ToString();
                }
            }

        public void SetPasswordHashAndSaltToEntity(string password)
        {
            Salt = GenerateSalt();
            Password = GeneratePasswordHashWithSalt(password, Salt);
        }
    }
}

