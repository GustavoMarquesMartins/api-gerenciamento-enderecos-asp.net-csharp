using AutoMapper;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GerenciamentoDeEndereco.Service
{
    public class CommonService
    {

        private readonly UserDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CommonService(UserDbContext db, IMapper mapper, IHttpContextAccessor httpContextAccessors)
        {
            this._db = db;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessors;
        }

        public async Task<User> getCurrentUserAsync()
        {
            var httpContextUser = _httpContextAccessor.HttpContext.User;

            if (!httpContextUser.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Usuário não autenticado.");
            }

            var claim = httpContextUser.FindFirst(ClaimTypes.NameIdentifier);

            if (!long.TryParse(claim.Value, out var id))
            {
                throw new InvalidOperationException("O valor da claim 'NameIdentifier' não é um número válido.");
            }

            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            return user;
        }

        public async Task<Uri> getUri<T>(T service, string id) where T : class
        {
            // variáveis do contexto da requisição
            var protocol = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host;
            var controller = service.GetType().Name.Replace("Controller", "");

            var uri = new Uri($"{protocol}://{host}/{controller}/{id}");

            return uri;
        }


        
    }
}
