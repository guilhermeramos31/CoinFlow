namespace CoinFlow.Models.Profiles;

using AutoMapper;
using UserEntity;
using UserEntity.Dto;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserRequest>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();
    }
}
