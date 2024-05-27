using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Adiciona AutoMapper ao contêiner de injeção de dependência
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<JwtService>(sp => new JwtService("1K5G3tj9QjSP56aEe2C3vrY9ZbFWd8xj"));

// Configuração do CORS
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeuProjeto v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Aplicando CORS ao pipeline
app.UseCors("AllowLocalhost");

app.MapControllers();

app.UseMiddleware<JwtAuthenticationMiddleware>("1K5G3tj9QjSP56aEe2C3vrY9ZbFWd8xj");

app.Run();
