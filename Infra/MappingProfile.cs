using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;

namespace GerenciamentoDeEndereco.Infra
    { 
        public class MappingProfile : Profile
        {
                public MappingProfile()
                {
                    // Mapeamento de Usuario para UsuarioDTO e vice-versa
                    CreateMap<User, UserDTO>().ReverseMap();
                    
                    CreateMap<User, UserResponse>().ReverseMap();

                    // Mapeamento de Endereco para EnderecoDTO e vice-versa
                    CreateMap<Address, AddressDTO>().ReverseMap();

                    CreateMap<Address, AddressResponse>().ReverseMap();
                }
        }
    }
