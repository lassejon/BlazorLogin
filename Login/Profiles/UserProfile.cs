using AutoMapper;
using Login.Dtos;
using Login.Models;

namespace Login.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ReadUserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<User, LoginUserResponseDto>();
    }
}