using AutoMapper;
using Impactt.Domain.Entities;
using Impactt.Service.DTOs;

namespace Impactt.Service.Mappers
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Room, RoomDTO>().ReverseMap();

            CreateMap<Room, RoomForCreationDto>().ReverseMap();

            CreateMap<User, UserForCreationDTO>().ReverseMap();

            CreateMap<UserRoomBook, UserRoomBookDTO>().ReverseMap();
        }
    }
}
