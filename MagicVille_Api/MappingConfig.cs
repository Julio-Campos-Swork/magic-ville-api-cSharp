using AutoMapper;
using MagicVille_Api.Modelos;
using MagicVille_Api.Modelos.Dto;

namespace MagicVille_Api
{
    public class MappingConfig : Profile
    {
        //en nuestro contructor generamos los mapeos de los updates que necesitemos
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<VillaCreateDto,Villa>().ReverseMap();
        }
    }
}
