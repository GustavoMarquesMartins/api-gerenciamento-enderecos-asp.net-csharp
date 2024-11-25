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
            // Convert DTO to user and save to the database
            var dtoToUser = _mapper.Map<User>(DTO);
            var user = await _db.Users.AddAsync(dtoToUser);
            await _db.SaveChangesAsync();

            // Generate response object
            var userResponse = _mapper.Map<UserResponse>(user.Entity);

            return userResponse;
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
            // Validate date fields
            dto.ValidateDate();
            // Get the authenticated user
            var user = await _commonService.GetCurrentUserAsync();

            // Update user properties if they are not null
            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.Password != null) user.Password = dto.Password;

            // Update user in the database
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }
    }
}
