using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeEndereco.Service
{
    public class AddressService
    {
        private readonly UserDbContext _db;
        private readonly IMapper _mapper;
        private readonly CommonService _commonService;

        /// <summary>
        /// Constructor that initializes AddressService with dependencies.
        /// </summary>
        /// <param name="userDbContext">User database context</param>
        /// <param name="mapper">Mapper for object-object mapping</param>
        /// <param name="commonService">Common service for shared functionalities</param>
        public AddressService(UserDbContext userDbContext, IMapper mapper, CommonService commonService)
        {
            _db = userDbContext;
            _mapper = mapper;
            _commonService = commonService;
        }

        /// <summary>
        /// Retrieves the list of addresses for the current authenticated user.
        /// </summary>
        /// <returns>List of AddressResponse objects</returns>
        public async Task<List<AddressResponse>> Get()
        {
            var currentUser = await _commonService.GetCurrentUserAsync();

            var addressList = await _db.Addresses
                .Where(e => e.UserId == currentUser.Id)
                .ToListAsync();

            var addressListResponse = addressList
                .Select(address => _mapper.Map<AddressResponse>(address))
                .ToList();

            return addressListResponse;
        }

        /// <summary>
        /// Retrieves a specific address by its ID for the current authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <returns>AddressResponse object</returns>
        /// <exception cref="Exception">Thrown when address is not found or does not belong to the user</exception>
        public async Task<AddressResponse> Get(long id)
        {
            var currentUser = await _commonService.GetCurrentUserAsync();

            var address = await _db.Addresses.FindAsync(id);

            if (address == null || address.UserId != currentUser.Id)
                throw new Exception("Address not found!");

            var addressResponse = _mapper.Map<AddressResponse>(address);

            return addressResponse;
        }

        /// <summary>
        /// Creates a new address for the current authenticated user.
        /// </summary>
        /// <param name="dto">Data Transfer Object for address creation</param>
        /// <returns>AddressResponse object with created address data</returns>
        public async Task<AddressResponse> Post(AddressDTO dto)
        {
            var address = _mapper.Map<Address>(dto);
            address.UserId = (await _commonService.GetCurrentUserAsync()).Id;

            var addressSaved = await _db.Addresses.AddAsync(address);
            await _db.SaveChangesAsync();

            var addressResponse = _mapper.Map<AddressResponse>(addressSaved.Entity);

            return addressResponse;
        }

        /// <summary>
        /// Deletes a specific address by its ID for the current authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when address is not found or does not belong to the user</exception>
        public async Task Delete(long id)
        {
            var currentUser = await _commonService.GetCurrentUserAsync();

            var address = await _db.Addresses.FindAsync(id);

            if (address == null || address.UserId != currentUser.Id)
                throw new Exception("Address not found!");

            _db.Addresses.Remove(address);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a specific address by its ID for the current authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <param name="dto">Data Transfer Object for address update</param>
        /// <returns>AddressResponse object with updated address data</returns>
        /// <exception cref="Exception">Thrown when address is not found or does not belong to the user</exception>
        public async Task<AddressResponse> Put(long id, AddressUpdateDTO dto)
        {
            dto.ValidateDate();
            var currentUser = await _commonService.GetCurrentUserAsync();
            var address = await _db.Addresses.FindAsync(id);

            if (address == null || address.UserId != currentUser.Id)
                throw new Exception("Address not found!");

            if (dto.ZipCode != null) address.ZipCode = dto.ZipCode;
            if (dto.Neighborhood != null) address.Neighborhood = dto.Neighborhood;
            if (dto.AdditionalInfo != null) address.AdditionalInfo = dto.AdditionalInfo;
            if (dto.Street != null) address.Street = dto.Street;
            if (dto.City != null) address.City = dto.City;
            if (dto.State != null) address.State = dto.State;
            if (dto.Number != null && dto.Number != 0) address.Number = dto.Number;

            _db.Addresses.Update(address);
            await _db.SaveChangesAsync();

            var addressResponse = _mapper.Map<AddressResponse>(address);

            return addressResponse;
        }
    }
}
