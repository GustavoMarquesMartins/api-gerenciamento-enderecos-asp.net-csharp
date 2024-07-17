using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});


// Adiciona serviços ao contêiner de injeção de dependência.
builder.Services.AddControllers(); // Adiciona serviços para controladores
builder.Services.AddEndpointsApiExplorer(); // Adiciona serviços para API Explorer
builder.Services.AddSwaggerGen(); // Configura o Swagger para geração de documentação

// Configuração do DbContext para a conexão com o banco de dados MySQL
builder.Services.AddDbContext<UserDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Adiciona AutoMapper ao contêiner de injeção de dependência para mapeamento de objetos
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registra o serviço JwtService com uma chave secreta específica
builder.Services.AddScoped<JwtService>(sp => new JwtService("1K5G3tj9QjSP56aEe2C3vrY9ZbFWd8xj"));

// Configuração do CORS para permitir requisições de qualquer origem, método e cabeçalho
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Exibe página de erro detalhada em ambiente de desenvolvimento
    app.UseSwagger(); // Configura o uso do Swagger para documentação da API
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeuProjeto v1")); // Configura a UI do Swagger
}

app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS

// Middleware de autenticação deve vir antes do middleware de autorização
app.UseAuthentication();
app.UseAuthorization();

// Aplica a política CORS configurada ao pipeline
app.UseCors("AllowLocalhost");

app.MapControllers(); // Mapeia os controladores para o pipeline de requisição

// Adiciona o middleware customizado JwtAuthenticationMiddleware ao pipeline
app.UseMiddleware<JwtAuthenticationMiddleware>("1K5G3tj9QjSP56aEe2C3vrY9ZbFWd8xj");

app.Run(); // Executa a aplicação
