
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
                    CreateMap<Usuario, UsuarioDTO>().ReverseMap();

                    // Mapeamento de Endereco para EnderecoDTO e vice-versa
                    CreateMap<Endereco, EnderecoDTO>().ReverseMap();

                    CreateMap<Endereco, EnderecoResponse>().ReverseMap();

                    CreateMap<Usuario, UsuarioResponse>().ReverseMap();
                }
        }
    }
