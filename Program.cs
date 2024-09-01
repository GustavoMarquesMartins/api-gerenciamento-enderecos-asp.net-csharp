using dotenv.net;
using GerenciamentoDeEndereco.Controllers;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Middlewares;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Carrega o arquivo .env
DotEnv.Load();

// Configura o arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Configura variáveis de ambiente
builder.Configuration.AddEnvironmentVariables();

// Recupera variáveis de ambiente e configurações
var smtpEmail = builder.Configuration["SMTP_EMAIL"];
var smtpPassword = builder.Configuration["SMTP_PASSWORD"];
var secretKey = builder.Configuration["CHAVE_SECRETA_APLICACAO"];
var dbHost = builder.Configuration["DATABASE_HOST"];
var dbPort = builder.Configuration["DATABASE_PORT"];
var dbName = builder.Configuration["DATABASE_NAME"];
var dbUser = builder.Configuration["DATABASE_USER"];
var dbPassword = builder.Configuration["DATABASE_PASSWORD"];

// Construa a string de conexão do banco de dados
var mySqlConnectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

// Configura os serviços
builder.Services.AddDbContext<UserDbContext>(options =>
{
    if (string.IsNullOrEmpty(mySqlConnectionString))
    {
        throw new InvalidOperationException("A string de conexão do MySqlConnection não foi encontrada.");
    }
    options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// Adiciona serviços ao contêiner de injeção de dependência
builder.Services.AddControllers(); // Adiciona serviços para controladores
builder.Services.AddEndpointsApiExplorer(); // Adiciona serviços para API Explorer
builder.Services.AddSwaggerGen(); // Configura o Swagger para geração de documentação
builder.Services.AddHttpContextAccessor(); // Adiciona seriço de contexto de usuário
//serviços
builder.Services.AddScoped<CommonService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<AuthenticationService>();

// Adiciona AutoMapper ao contêiner de injeção de dependência
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registra o serviço JwtService com a chave secreta
builder.Services.AddScoped<JwtService>(sp => new JwtService(secretKey));

// Configuração do CORS para permitir requisições de qualquer origem, método e cabeçalho
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.AllowAnyOrigin()
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

app.UseAuthentication(); // Middleware de autenticação deve vir antes do middleware de autorização
app.UseAuthorization();

// Aplica a política CORS configurada ao pipeline
app.UseCors("AllowLocalhost");

app.MapControllers(); // Mapeia os controladores para o pipeline de requisição

// Adiciona o middleware customizado JwtAuthenticationMiddleware ao pipeline
app.UseMiddleware<JwtAuthenticationMiddleware>(secretKey);

app.UseStaticFiles(); // Permite que a aplicação ultilze de arquivos estáticos

app.Run(); // Executa a aplicação
