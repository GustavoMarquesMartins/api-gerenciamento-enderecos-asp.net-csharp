using AddressManagement.Infra;
using AddressManagement.Model;
using AddressManagement.Service;
using AutoMapper;
using GerenciamentoDeEndereco.CustomExceptions;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeEndereco.Service
{
    /// <summary>
    /// Service class for managing user-related operations.
    /// </summary>
    public class UserService
    {
        private readonly UserDbContext _db;
        private readonly IMapper _mapper;
        private readonly CommonService _commonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with the specified dependencies.
        /// </summary>
        /// <param name="dbContext">The database context</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="commonService">The common service for shared operations</param>
        public UserService(UserDbContext dbContext, IMapper mapper, CommonService commonService)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._commonService = commonService;
        }

        /// <summary>
        /// Retrieves the authenticated user.
        /// </summary>
        /// <returns>UserResponse object with authenticated user data</returns>
        public async Task<UserResponse> Get()
        {
            // Get the authenticated user in the request context.
            var userAuthenticated = await _commonService.GetCurrentUserAsync();
            // Convert the user object to a response object.
            var userResponse = _mapper.Map<UserResponse>(userAuthenticated);
            // Return the response

            return userResponse;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="DTO">Data Transfer Object for user creation</param>
        /// <returns>UserResponse object with created user data</returns>
        public async Task<UserResponse> Post(UserDTO DTO)
        {
            DTO.ValidateData();

            var result = await GetUserByEmailAsync(DTO.Email);
            if (result != null) throw new EmailAlreadyRegisteredException("The provided email is already registered.");

            // Convert DTO to user and save to the database
            var user = _mapper.Map<User>(DTO);

            // hashes the password
            user.SetPasswordHashAndSaltToEntity(user.Password);

            var userSaved = await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            // Generate response object
            var userResponse = _mapper.Map<UserResponse>(userSaved.Entity);

            return userResponse;
        }

        /// <summary>
        /// Retrieves a user by their email asynchronously.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The user if found, otherwise throws an exception.</returns>
        /// <exception cref="Exception">Thrown when the user with the provided email is not found.</exception>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _db.Users
                .Include(user => user.PasswordResetTokens)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        /// <summary>
        /// Deletes the authenticated user.
        /// </summary>
        public async Task Delete()
        {
            // Get the authenticated user
            var currentUser = await _commonService.GetCurrentUserAsync();
            // Remove the user from the database
            _db.Users.Remove(currentUser);
            // Save changes to the database
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the authenticated user's details.
        /// </summary>
        /// <param name="dto">Data Transfer Object for user update</param>
        /// <returns>UserResponse object with updated user data</returns>
        public async Task<UserResponse> Put(UserUpdateDTO dto)
        {
            // Validate data fields
            dto.ValidateData();
            // Get the authenticated user
            var user = await _commonService.GetCurrentUserAsync();

            // Update user properties if they are not null
            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Email != null) user.Email = dto.Email;

            if (dto.Password != null) {
                user.SetPasswordHashAndSaltToEntity(dto.Password);
            }

            // Update user in the database
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }
    }
}
