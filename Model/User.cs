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

        /// <summary>
        /// Representa o salt para a senha e métodos para gerar o salt e o hash da senha.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Gera um salt aleatório de 16 bytes e o converte para uma string Base64.
        /// O salt é utilizado para garantir que senhas iguais gerem hashes diferentes.
        /// </summary>
        /// <returns>A string do salt gerado em Base64.</returns>
        public string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16]; // 16 bytes for the salt
                rng.GetBytes(saltBytes); // Preenche o array com valores aleatórios
                Salt = Convert.ToBase64String(saltBytes); // Converte o salt para uma string Base64
                return Salt;
            }
        }

        /// <summary>
        /// Gera o hash da senha concatenada com o salt utilizando o algoritmo SHA256.
        /// O hash é uma representação segura da senha e salt, sendo usado para verificar a autenticidade da senha.
        /// </summary>
        /// <param name="password">A senha do usuário.</param>
        /// <param name="salt">O salt gerado para a senha.</param>
        /// <returns>A string do hash gerado em formato hexadecimal.</returns>
        public string GeneratePasswordHashWithSalt(string password, string salt)
        {
            string passwordWithSalt = password + salt;

            // Hash a senha com o salt
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                byte[] hashBytes = SHA256.HashData(passwordBytes);

                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2")); // Converte cada byte para uma string hexadecimal
                }

                return hashStringBuilder.ToString();
            }
        }

        /// <summary>
        /// Gera e atribui o salt e o hash da senha à entidade.
        /// Este método chama os métodos de geração de salt e de hash da senha, e armazena os valores resultantes.
        /// </summary>
        /// <param name="password">A senha do usuário.</param>
        public void SetPasswordHashAndSaltToEntity(string password)
        {
            Salt = GenerateSalt();
            Password = GeneratePasswordHashWithSalt(password, Salt);
        }
    }
}

