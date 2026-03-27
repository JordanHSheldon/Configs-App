namespace EsportsProfileWebApi.Web.Mapping;

using AutoMapper;
using Controllers.DTOs.User;
using Controllers.DTOs;
using Orchestrators.Models.User;
using Repository;
using Responses.User;
using EsportsProfileWebApi.Web.Orchestrators.Models.Profile;
using EsportsProfileWebApi.Web.Repository.Entities;
using EsportsProfileWebApi.Web.Orchestrators.Models.Peripheral;
using EsportsProfileWebApi.Web.Controllers.DTOs.Profile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GetProfileByNameRequestModel,GetProfileByNameRequestDTO>().ReverseMap();
        CreateMap<GetProfileResponseDTO, GetProfileResponseModel>().ReverseMap();
        CreateMap<GetProfileByNameRequestDTO, GetProfileRequestModel>().ReverseMap();
        CreateMap<UpdateProfileResponseDTO, UpdateProfileResponseModel>().ReverseMap();
        CreateMap<UpdateProfileResponseModel, bool>().ReverseMap();
        CreateMap<UserLoginRequestDTO, UserLoginRequestModel>().ReverseMap();
        CreateMap<UpdateProfileRequestDTO, UpdateProfileRequest>();
        CreateMap<UserRegisterRequestDTO, UserRegisterRequestModel>().ReverseMap();
        CreateMap<UserRegisterResponseDTO, UserRegisterResponseModel>().ReverseMap();
        CreateMap<UserLoginResponseDTO, UserLoginResponseModel>().ReverseMap();
        CreateMap<GetProfileResponseModel, ProfileEntity>().ReverseMap();
        CreateMap<ProfileEntity, GetPaginatedUsersResponseModel>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<GetPaginatedUsersRequestDTO,GetPaginatedUsersRequestModel>().ReverseMap();
        CreateMap<GetPaginatedUsersResponseDto,GetPaginatedUsersResponseModel>().ReverseMap();
        CreateMap<PeripheralDto,PeripheralModel>().ReverseMap();
        CreateMap<PeripheralModel,PeripheralEntity>().ReverseMap();
    }
}