using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;

namespace GerenciamentoDeEndereco.Infra
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor that initializes the mapping profiles.
        /// </summary>
        public MappingProfile()
        {
            // Mapping from User to UserDTO and vice versa
            CreateMap<User, UserDTO>().ReverseMap();

            // Mapping from User to UserResponse and vice versa
            CreateMap<User, UserResponse>().ReverseMap();

            // Mapping from Address to AddressDTO and vice versa
            CreateMap<Address, AddressDTO>().ReverseMap();

            // Mapping from Address to AddressResponse and vice versa
            CreateMap<Address, AddressResponse>().ReverseMap();
        }
    }
}
