using GerenciamentoDeEndereco.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GerenciamentoDeEndereco.Infra
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Gets or sets the Users DbSet.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Addresses DbSet.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Constructor that initializes the UserDbContext with configuration and options.
        /// </summary>
        /// <param name="configuration">Configuration for the context</param>
        /// <param name="options">Options for the context</param>
        /// <exception cref="ArgumentNullException">Thrown when configuration is null</exception>
        public UserDbContext(IConfiguration configuration, DbContextOptions<UserDbContext> options)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configures the entity framework model.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the User entity to make the email field unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
