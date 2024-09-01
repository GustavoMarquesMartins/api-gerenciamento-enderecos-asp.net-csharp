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

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


        public UserDbContext(IConfiguration configuration, DbContextOptions<UserDbContext> options)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura a entidade User para que o campo email seja único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
