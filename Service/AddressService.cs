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


        public AddressService(UserDbContext userDbContext, IMapper mapper, CommonService commonService)
        {
             _db = userDbContext;
            _mapper = mapper;  
            _commonService = commonService;
        }

        public async Task<List<AddressResponse>> get()
        {
                var currentUser = await _commonService.getCurrentUserAsync();

                var addressList = await _db.Addresses
                    .Where(e => e.UserId == currentUser.Id)
                    .ToListAsync();

                var addressListResponse = addressList
                    .Select(endereco => _mapper.Map<AddressResponse>(endereco))
                    .ToList();

                return addressListResponse;
        }

        public async Task<AddressResponse> get(long id)
        {
                var currentUser = await _commonService.getCurrentUserAsync();

                var address = await _db.Addresses.FindAsync(id);

                if (address == null || address.UserId != currentUser.Id)
                    throw new Exception("Endereço não encontrado!");

                var addressResponse = _mapper.Map<AddressResponse>(address);

            return addressResponse;
        }

        public async Task<AddressResponse> post(AddressDTO dto)
        {
                var address = _mapper.Map<Address>(dto);
                address.UserId = (await _commonService.getCurrentUserAsync()).Id;

                var addressSaved = await _db.Addresses.AddAsync(address);
                await _db.SaveChangesAsync();

                var addressResponse = _mapper.Map<AddressResponse>(addressSaved.Entity);

                return addressResponse;
        }

        public async Task delete(long id)
        {
                var currentUser = await _commonService.getCurrentUserAsync();

                var address = await _db.Addresses.FindAsync(id);

                if (address == null || (address.UserId != currentUser.Id)) throw new Exception("Endereço não encontrado!");

                _db.Addresses.Remove(address);
                await _db.SaveChangesAsync();
            }

        public async Task<AddressResponse> put(long id, AddressUpdateDTO dto)
        {
                dto.validateDate();
                var currentUser = await _commonService.getCurrentUserAsync();
                var address = await _db.Addresses.FindAsync(id);

                if ((address == null) || (address.UserId != currentUser.Id)) throw new Exception("Endereço não encontrado!");

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