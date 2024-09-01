using AutoMapper;
using Azure.Core;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeEndereco.Service
{
    public class EmailService
    {
        private readonly UserDbContext _db;
        private readonly IMapper _mapper;

        public EmailService(UserDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;  
        }
        
        public async Task<String> requestPasswordReset([FromBody] ForgotPasswordDTO resetPassword)
        {
            resetPassword.ValidaDadosEntrada();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == resetPassword.Email);

            if (user == null) throw new Exception("Usuário não encontrado.");

            var Token = TokenGenerator.GenerateToken();
            var expiration = DateTime.UtcNow.AddHours(1);

            var TokenService = new PasswordResetToken()
                .setToken(Token)
                .setEmail(resetPassword.Email)
                .setExpiration(expiration);

            var passwordResetToken = await _db.PasswordResetTokens.AddAsync(TokenService);
            await _db.SaveChangesAsync();

            return passwordResetToken.Entity.Token;
        }

        private async Task<PasswordResetToken> VerifyTokenValidity(string Token)
        {
            if (string.IsNullOrEmpty(Token)) throw new ArgumentNullException("Token não pode ser um campo em branco.");

            var passwordResetToken = await _db.PasswordResetTokens.FirstOrDefaultAsync(u => u.Token == Token);
            if (passwordResetToken == null) throw new Exception("Token inválido");

            if (passwordResetToken.Expiration < DateTime.Now) throw new Exception("Token expirado. Favor fazer a solicitação novamente!");

            return passwordResetToken;
        }

        public async Task<UserResponse> resetPassword([FromBody] ResetPasswordDTO data)
        {
            data.validaDadosEntrada();
            var passwordResetToken = await this.VerifyTokenValidity(data.Token);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == passwordResetToken.Email);

            if (user == null) throw new Exception("Usuário relacionado ao Token não encontrado.");

            if (user.Password == data.NewPassword) throw new Exception("A nova senha tem que ser diferente da senha antiga");

            user.Password = data.NewPassword;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            var userResponse = _mapper.Map<UserResponse>(user);
            return userResponse;
        }

    }
}
