using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Migrations;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeEndereco.Service
{
    public class UserService
    {

        private readonly UserDbContext _db;
        private readonly IMapper _mapper;
        private readonly CommonService _commonService;

        public UserService(UserDbContext dbContext, IMapper mapper, CommonService commonService, EmailService EmailService)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._commonService = commonService;
        }
        public async Task<UserResponse> get()
        {
            var userAuthenticated = await _commonService.getCurrentUserAsync(); // busca o usuário autenticado no contexto da requisição.
            var userResponse = _mapper.Map<UserResponse>(userAuthenticated); // converte um objeto do tipo usuário para um objeto de resposta.
            return userResponse; // retorna a resposta
        }

        public async Task<UserResponse> post(UserDTO DTO)
        {
                //converte dto para usuário e salva no banco de dados
                var dtoToUser = _mapper.Map<User>(DTO);
                var user = await _db.Users.AddAsync(dtoToUser);
                await _db.SaveChangesAsync();

                // gera objeto de resposta
                var userResponse = _mapper.Map<UserResponse>(user.Entity);

                return userResponse;
        }
        
        public async Task delete()
        {
            var currentUser = await _commonService.getCurrentUserAsync();
            _db.Users.Remove(currentUser);
            await _db.SaveChangesAsync();
        }
        
        public async Task<UserResponse> put(UserUpdateDTO dto)
        {
            dto.validateDate();
            var usuario = await _commonService.getCurrentUserAsync();

            if (dto.Name != null) usuario.Name = dto.Name;
            if (dto.Email != null) usuario.Email = dto.Email;
            if (dto.Password != null) usuario.Password = dto.Password;

            _db.Users.Update(usuario);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserResponse>(usuario);
        }
    }
}
