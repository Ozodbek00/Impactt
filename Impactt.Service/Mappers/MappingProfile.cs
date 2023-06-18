using AutoMapper;
using Impactt.Domain.Entities;
using Impactt.Service.DTOs;

namespace Impactt.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Room, RoomDTO>().ReverseMap();

            CreateMap<UserRoomBook, UserRoomBookDTO>().ReverseMap();
        }
    }
}
