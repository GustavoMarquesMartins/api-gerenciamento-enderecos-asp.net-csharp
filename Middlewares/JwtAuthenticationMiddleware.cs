using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GerenciamentoDeEndereco.Middlewares
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _key;

        /// <summary>
        /// Constructor that initializes the JWT authentication middleware with dependencies.
        /// </summary>
        /// <param name="next">The next middleware delegate</param>
        /// <param name="key">Secret key for JWT validation</param>
        public JwtAuthenticationMiddleware(RequestDelegate next, string key)
        {
            _next = next;
            _key = key;
        }

        /// <summary>
        /// Middleware invocation to handle JWT authentication.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/Authentication") && context.Request.Method == "POST")
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/User") && context.Request.Method == "POST")
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header is missing");
                return;
            }

            var token = authHeader.ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                context.User = principal;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token: " + ex.Message);
                return;
            }

            await _next(context);
        }
    }
}
