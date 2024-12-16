using AddressManagement.Infra;
using AddressManagement.Model;
using AutoMapper;
using GerenciamentoDeEndereco.CustomExceptions;
using System.Security.Claims;

namespace AddressManagement.Service
{
    /// <summary>
    /// Service class for common functionalities.
    /// </summary>
    public class CommonService
    {
        private readonly UserDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor that initializes CommonService with dependencies.
        /// </summary>
        /// <param name="db">User database context</param>
        /// <param name="httpContextAccessors">Accessor for HTTP context</param>
        public CommonService(UserDbContext db, IMapper mapper, IHttpContextAccessor httpContextAccessors)
        {
            this._db = db;
            this._httpContextAccessor = httpContextAccessors;
        }

        /// <summary>
        /// Retrieves the current authenticated user asynchronously.
        /// </summary>
        /// <returns>User object representing the current authenticated user</returns>
        /// <exception cref="UserNotAuthenticated">Thrown when the user is not authenticated</exception>
        /// <exception cref="InvalidClaimIdentifierException">Thrown when the claim identifier is invalid</exception>
        /// <exception cref="UserNotFound">Thrown when the user is not found in the database</exception>
        public async Task<User> GetCurrentUserAsync()
        {
            var httpContextUser = _httpContextAccessor.HttpContext.User;

            if (!httpContextUser.Identity.IsAuthenticated)
            {
                throw new UserNotAuthenticated("User not authenticated.");
            }

            var claim = httpContextUser.FindFirst(ClaimTypes.NameIdentifier);

            if (!long.TryParse(claim.Value, out var id))
            {
                throw new InvalidClaimIdentifierException("The claim value 'NameIdentifier' is not a valid number.");
            }

            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                throw new UserNotFound("User not found.");
            }

            return user;
        }

        /// <summary>
        /// Generates a URI for a given service and identifier.
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <param name="service">Service instance</param>
        /// <param name="id">Identifier for the URI</param>
        /// <returns>Generated URI</returns>
        public Uri GetUri<T>(T service, string id) where T : class
        {
            // Request context variables
            var protocol = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host;
            var controller = service.GetType().Name.Replace("Controller", "");

            var uri = new Uri($"{protocol}://{host}/{controller}/{id}");

            return uri;
        }
    }
}
