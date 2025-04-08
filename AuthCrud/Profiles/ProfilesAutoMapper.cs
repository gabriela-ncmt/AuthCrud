using AuthCrud.Dto.Usuario;
using AuthCrud.Models;
using AutoMapper;

namespace AuthCrud.Profiles
{
    public class ProfilesAutoMapper : Profile
    {
        public ProfilesAutoMapper()
        {
            CreateMap<UsuarioCriacaoDto, UsuarioModel>().ReverseMap();
        }
    }
}
