using AutoMapper;
using TinderForPets.Data.Entities;
using TinderForPets.Core.Models;

namespace TinderForPets.Data.Configurations.AutoMapper
{
    public class MapperConfig : Profile
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserAccount>();
                cfg.CreateMap<UserAccount, User>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
