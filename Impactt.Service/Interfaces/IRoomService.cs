using Impactt.Domain.Entities;
using Impactt.Service.DTOs;
using System.Linq.Expressions;

namespace Impactt.Service.Interfaces
{
    public interface IRoomService
    {
        /// <summary>
        /// Get room by id and date.
        /// </summary>
        Task<UserRoomBookDTO[]> GetRoomFreeIntervalsAsync(long  roomId, DateTime date);

        /// <summary>
        /// Book room.
        /// </summary>
        Task BookRoomAsync(long roomId, ReservationDTO reservationDTO);

        /// <summary>
        /// Create room.
        /// </summary>
        Task<RoomDTO> CreateAsync(RoomForCreationDto roomDTO);

        /// <summary>
        /// Update room.
        /// </summary>
        Task<RoomDTO> UpdateAsync(long id, RoomForCreationDto roomDTO);

        /// <summary>
        /// Delete room.
        /// </summary>
        Task DeleteAsync(long id);

        /// <summary>
        /// Get room by expression.
        /// </summary>
        Task<RoomDTO> GetAsync(Expression<Func<Room, bool>> expression);

        /// <summary>
        /// Get rooms with expression and pagination.
        /// </summary>
        Task<RoomDTO[]> GetAllAsync(int pageIndex, int pageSize, Expression<Func<Room, bool>> expression = null);
    }
}
