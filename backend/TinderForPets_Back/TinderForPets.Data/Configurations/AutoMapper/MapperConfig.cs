using AutoMapper;
using TinderForPets.Data.Entities;
using TinderForPets.Core.Models;

namespace TinderForPets.Data.Configurations.AutoMapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserAccount>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserAccount, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));
        }
    }
}
