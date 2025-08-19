using AutoMapper;
using Q_A_Backend.DTOs;
using Q_A_Backend.Models;

namespace Q_A_Backend.Mapper
{
    public class MappingProfile : Profile
    {
         public MappingProfile()
        {
            CreateMap<User, UserForAdminDto>();
            CreateMap<User, UserForAdminDto>(); // Map properties with same names
        }
        
    }
}