using AutoMapper;
using AutoMapper.QueryableExtensions;
using Impactt.Data.Interfaces;
using Impactt.Domain.Entities;
using Impactt.Service.DTOs;
using Impactt.Service.Exceptions;
using Impactt.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Impactt.Service.Services
{
    public sealed class RoomService : IRoomService
    {
        private readonly IRepository<Room> repository;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<UserRoomBook> userBookRepo;
        private readonly IMapper mapper;

        public RoomService(IRepository<Room> repository,
                           IRepository<User> userRepo,
                           IRepository<UserRoomBook> userBookRepo, 
                           IMapper mapper)
        {
            this.repository = repository;
            this.userRepo = userRepo;
            this.userBookRepo = userBookRepo;
            this.mapper = mapper;
        }

        /// <summary>
        /// Create room.
        /// </summary>
        public async Task<RoomDTO> CreateAsync(RoomForCreationDto roomDTO)
        {
            var room = await repository.GetAsync(expression: s =>
                  s.Name.Equals(roomDTO.Name));

            if (room is not null)
                throw new ImpacttException(400, "Room with this name exists");

            Room mappedRoom = mapper.Map<Room>(roomDTO);
            mappedRoom.CreatedAt = DateTime.UtcNow;

            await repository.AddAsync(mappedRoom);

            return mapper.Map<RoomDTO>(mappedRoom);
        }

        /// <summary>
        /// Get room by id and date.
        /// </summary>
        public async Task<UserRoomBookDTO[]> GetRoomFreeIntervalsAsync(long roomId, DateTime date)
        {
            var room = await repository.GetAsync(expression: s => s.Id == roomId);

            if (room is null)
                throw new ImpacttException(404, "Room with this name does not exist");

            if (date < DateTime.UtcNow)
                date = DateTime.UtcNow;

            var bookings = userBookRepo.GetAll(1, 50, 
                expression: u => u.RoomId == roomId && 
                            u.StartAt.Date.Year == date.Year && 
                            u.StartAt.Date.Month == date.Month &&
                            u.StartAt.Date.Day == date.Day).OrderBy(b => b.StartAt);

            if (!bookings.Any())
                return new UserRoomBookDTO[] 
                {
                    new UserRoomBookDTO 
                    {
                        StartAt = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0),
                        EndAt = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0)
                    }
                };

            DateTime start = new(date.Year, date.Month, date.Day, 9, 0, 0);
            List<UserRoomBookDTO> intervals = new();

            foreach (var booking in bookings)
            {
                if (booking.StartAt <= date)
                    start = booking.EndAt;
                else
                {
                    intervals.Add(new UserRoomBookDTO() { StartAt = start, EndAt = booking.StartAt });
                }

                start = booking.EndAt;
            }

            var last = intervals.LastOrDefault();

            if (last == null)
            {
                intervals.Add(new UserRoomBookDTO()
                {
                    StartAt = start,
                    EndAt = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0)
                });
            }
            else if (last.EndAt.Hour < 23)
            {
                intervals.Add(new UserRoomBookDTO()
                {
                    StartAt = start,
                    EndAt = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0)
                });
            }

            return intervals.ToArray();
        }

        /// <summary>
        /// Book room.
        /// </summary>
        public async Task BookRoomAsync(long roomId, ReservationDTO reservationDTO)
        {
            await GetAsync(expression: r => r.Id == roomId);

            var user = await userRepo.GetAsync(expression: u
                    => string.Concat(u.FirstName, " ", u.LastName).Equals(reservationDTO.Booker.Name));

            if (user == null)
                throw new ImpacttException(404, "Bunday inson ro'yxatda yo'q");

            var intervals = (await GetRoomFreeIntervalsAsync(roomId, reservationDTO.StartAt))
                   .Any(i => i.StartAt <= reservationDTO.StartAt && i.EndAt >= reservationDTO.EndAt);

            if (!intervals)
                throw new ImpacttException(410, "Uzr, siz tanlagan vaqtda xona band");

            UserRoomBook roomBook = new()
            {
                RoomId = roomId,
                UserId = user.Id,
                StartAt = reservationDTO.StartAt,
                EndAt = reservationDTO.EndAt,
                CreatedAt = DateTime.UtcNow
            };

            await userBookRepo.AddAsync(roomBook);
        }

        /// <summary>
        /// Update room.
        /// </summary>
        public async Task<RoomDTO> UpdateAsync(long id, RoomForCreationDto roomDTO)
        {
            var room = await repository.GetAsync(expression: s => s.Id == id);

            if (room is null)
                throw new ImpacttException(404, "Room with this name does not exist");

            Room mappedRoom = mapper.Map(roomDTO, room);
            mappedRoom.CreatedAt = room.CreatedAt;
            mappedRoom.UpdatedAt = DateTime.UtcNow;

            await repository.UpdateAsync(mappedRoom);

            return mapper.Map<RoomDTO>(mappedRoom);
        }

        /// <summary>
        /// Delete room.
        /// </summary>
        public async Task DeleteAsync(long id)
        {
            var room = await repository.GetAsync(expression: s => s.Id == id);

            if (room == null)
                throw new ImpacttException(404, "Room not found");

            await repository.DeleteAsync(room);
        }

        /// <summary>
        /// Get room by expression.
        /// </summary>
        public async Task<RoomDTO> GetAsync(Expression<Func<Room, bool>> expression)
        {
            var room = await repository.GetAsync(expression);

            if (room == null)
                throw new ImpacttException(404, "Room not found");

            return mapper.Map<RoomDTO>(room);
        }

        /// <summary>
        /// Get rooms with expression and pagination.
        /// </summary>
        public async Task<RoomDTO[]> GetAllAsync(int pageIndex, int pageSize, Expression<Func<Room, bool>> expression = null)
        {
            return await repository.GetAll(pageIndex, pageSize, expression)
                .ProjectTo<RoomDTO>(mapper.ConfigurationProvider).ToArrayAsync();
        }
    }
}
