using System.Security.Authentication;
using AddressManagement.Infra;
using AddressManagement.Model;
using GerenciamentoDeEndereco.CustomExceptions;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AddressManagement.Service
{
    /// <summary>
    /// Service class for handling authentication-related operations.
    /// </summary>
    public class AuthenticationService
    {
        private readonly UserDbContext _db;
        private readonly JwtService _jwtService;

        /// <summary>
        /// Constructor that initializes the AuthenticationService with dependencies.
        /// </summary>
        /// <param name="db">User database context</param>
        /// <param name="jwtService">Service for generating JWT tokens</param>
        public AuthenticationService(UserDbContext db, JwtService jwtService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _jwtService = jwtService;
        }

        /// <summary>
        /// Authenticates a user with provided credentials and generates a JWT token.
        /// </summary>
        /// <param name="dto">Login Data Transfer Object containing user credentials</param>
        /// <returns>JWT token as a string</returns>
        /// <exception cref="InvalidCredentialsException">Thrown when the provided credentials are incorrect</exception>
        public async Task<string> Post([FromBody] LoginDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || user.Password != dto.Password)
            {
                throw new InvalidCredentialsException("Incorrect E-mail or password. Please check your credentials and try again.");
            }

            var token = _jwtService.GenerateToken(user.Id.ToString());
            return token;
        }
    }
}
