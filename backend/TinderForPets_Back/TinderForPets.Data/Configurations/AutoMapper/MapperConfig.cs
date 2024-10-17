using AutoMapper;
using dataLayer = TinderForPets.Data.Entities;
using applicationLayer = TinderForPets.Core.Models;

namespace TinderForPets.Data.Configurations.AutoMapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<applicationLayer.User, dataLayer.UserAccount>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));

            CreateMap<dataLayer.UserAccount, applicationLayer.User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));

            CreateMap<dataLayer.AnimalProfile, applicationLayer.AnimalProfile>();
            CreateMap<applicationLayer.AnimalProfile, dataLayer.AnimalProfile>();

        }
    }
}
