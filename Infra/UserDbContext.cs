using GerenciamentoDeEndereco.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Validations;
using System;

namespace GerenciamentoDeEndereco.Infra
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


        public UserDbContext(IConfiguration configuration, DbContextOptions<UserDbContext> options)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
