using AutoMapper;
using dataLayer = TinderForPets.Data.Entities;
using applicationLayer = TinderForPets.Core.Models;

namespace TinderForPets.Infrastructure
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<applicationLayer.User, dataLayer.UserAccount>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));

            CreateMap<dataLayer.UserAccount, applicationLayer.User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));

            CreateMap<dataLayer.Animal, applicationLayer.AnimalModel>();
            CreateMap<applicationLayer.AnimalModel, dataLayer.Animal>();


            CreateMap<dataLayer.AnimalProfile, applicationLayer.AnimalProfileModel>();
            CreateMap<applicationLayer.AnimalProfileModel, dataLayer.AnimalProfile>();

            CreateMap<dataLayer.AnimalImage, applicationLayer.AnimalImageModel>();
            CreateMap<applicationLayer.AnimalImageModel, dataLayer.AnimalImage>();
        }
    }
}
