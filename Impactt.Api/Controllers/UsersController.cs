using Impactt.Service.DTOs;
using Impactt.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Impactt.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserDTO dto)
        {
            return Ok(await userService.CreateAsync(dto));
        }

        /// <summary>
        /// Get all users with pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageIndex, int pageSize)
        {
            return Ok(await userService.GetAllAsync(pageIndex, pageSize));
        }

        /// <summary>
        /// Get specific user by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await userService.GetAsync(u => u.Id == id));
        }

        /// <summary>
        /// Update user by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, UserDTO userDto)
        {
            return Ok(await userService.UpdateAsync(id, userDto));
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            return Ok(userService.DeleteAsync(id));
        }
    }
}
