using AddressManagement.Middlewares;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AddressManagement.Middlewares
{
    /// <summary>
    /// Middleware class for handling JWT authentication.
    /// </summary>
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
            // Skip JWT validation for specific endpoints and HTTP methods
            if (PathSettings.IsExcludedPath(context))
            {
                await _next(context);
                return;
            }

            // Check if the Authorization header is present
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization header is missing");
                return;
            }

            // Extract the token from the Authorization header
            var token = authHeader.ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // No clock skew to allow for precise token expiry validation
            };

            try
            {
                // Validate the token and set the principal in the context
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                context.User = principal;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid token: " + ex.Message);
                return;
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
