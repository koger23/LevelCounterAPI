using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Models.DTO;

namespace LevelCounter.Configs
{
    public class MappingProfiles
    {
        public static MapperConfiguration GetAutoMapperProfiles()
        {
            return new MapperConfiguration(mc =>
            {
                mc.CreateMap<ApplicationUser, UserResponse>()
                    .ForMember(dest => dest.Gender, opts => opts.MapFrom(src => src.Sex));
                mc.CreateMap<SignupRequest, ApplicationUser>()
                    .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FullName))
                    .ForMember(dest => dest.Statistics, opts => opts.MapFrom(src => new Statistics()));
            });
        }
    }
}
