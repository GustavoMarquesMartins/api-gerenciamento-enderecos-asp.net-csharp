using AddressManagement.Infra;
using AddressManagement.Service;
using dotenv.net;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Security.Middlewares;
using GerenciamentoDeEndereco.Service;
using GerenciamentoDeEndereco.Validator;
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

// Loads the .env file
DotEnv.Load();

// Configures the appsettings.json file
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Configures environment variables
builder.Configuration.AddEnvironmentVariables();

// Retrieves environment variables and settings
var smtpEmail = builder.Configuration["SMTP_EMAIL"];
var smtpAppPassword = builder.Configuration["SMTP_APP_PASSWORD"];
var secretKey = builder.Configuration["CHAVE_SECRETA_APLICACAO"];
var dbHost = builder.Configuration["DATABASE_HOST"];
var dbPort = builder.Configuration["DATABASE_PORT"];
var dbName = builder.Configuration["DATABASE_NAME"];
var dbUser = builder.Configuration["DATABASE_USER"];
var dbPassword = builder.Configuration["DATABASE_PASSWORD"];

EnvironmentVariableChecker.Validate();

// Builds the database connection string
var mySqlConnectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

// Configures services
builder.Services.AddDbContext<UserDbContext>(options =>
{
    if (string.IsNullOrEmpty(mySqlConnectionString))
    {
        throw new InvalidOperationException("The MySqlConnection connection string was not found.");
    }
    options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString));
});

builder.Services.AddScoped<EmailSettings>(config => {
    return new EmailSettings(smtpEmail, smtpAppPassword);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// Adds services to the dependency injection container
builder.Services.AddControllers(); // Adds services for controllers
builder.Services.AddEndpointsApiExplorer(); // Adds services for API Explorer
builder.Services.AddSwaggerGen(); // Configures Swagger for API documentation
builder.Services.AddHttpContextAccessor(); // Adds user context service

// Services
builder.Services.AddScoped<CommonService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<PasswordResetService>(provider =>
{
    var context = provider.GetRequiredService<UserDbContext>();
    var emailSettings = provider.GetRequiredService<EmailSettings>();
    var userService = provider.GetRequiredService<UserService>();
    return new PasswordResetService(context, emailSettings, userService);
});

// Adds AutoMapper to the dependency injection container
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registers the JwtService with the secret key
builder.Services.AddScoped<JwtService>(sp => new JwtService(secretKey));

// Configures CORS to allow requests from any origin, method, and header
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

// Configures the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Displays detailed error page in development environment
    app.UseSwagger(); // Configures Swagger for API documentation
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject v1")); // Configures Swagger UI
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS

app.UseAuthentication(); // Authentication middleware must come before authorization middleware
app.UseAuthorization();

// Applies the configured CORS policy to the pipeline
app.UseCors("AllowLocalhost");

app.MapControllers(); // Maps controllers to the request pipeline

// Adds the custom JwtAuthenticationMiddleware to the pipeline
app.Use(async (context, next) =>
{
    var middleware = new JwtAuthenticationMiddleware(next, secretKey);
    await middleware.InvokeAsync(context);
});

app.UseStaticFiles(); // Allows the application to use static files

app.Run(); // Runs the application
