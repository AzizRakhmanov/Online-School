using AutoMapper;
using Domain.Models;
using Service.Dto;

namespace Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserForCreationDto>().ReverseMap();
            CreateMap<User, UserForResultDto>().ReverseMap();
            CreateMap<UserForCreationDto, UserForResultDto>().ReverseMap();
        }
    }
}
