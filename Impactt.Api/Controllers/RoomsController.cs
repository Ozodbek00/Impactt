using Impactt.Service.DTOs;
using Impactt.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Impactt.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService roomService;

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        /// <summary>
        /// Get room's free intervals.
        /// </summary>
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<UserRoomBookDTO[]>> GetRoomIntervalsAsync([FromRoute]long id, DateTime date)
        {
            return Ok(await roomService.GetRoomFreeIntervalsAsync(id, date));
        }

        /// <summary>
        /// Book room.
        /// </summary>
        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookRoomAsync([FromRoute] long id, ReservationDTO reservationDTO)
        {
            await roomService.BookRoomAsync(id, reservationDTO);

            return Ok();
        }

        /// <summary>
        /// Create roomDto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoomDTO roomDto)
        {
            return Ok(await roomService.CreateAsync(roomDto));
        }

        /// <summary>
        /// Get all rooms with pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageIndex, int pageSize)
        {
            return Ok(await roomService.GetAllAsync(pageIndex, pageSize));
        }

        /// <summary>
        /// Get room by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await roomService.GetAsync(b => b.Id == id));
        }

        /// <summary>
        /// Update room by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, RoomDTO roomDto)
        {
            return Ok(await roomService.UpdateAsync(id, roomDto));
        }

        /// <summary>
        /// Delete room by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await roomService.DeleteAsync(id);

            return Ok();
        }
    }
}
