using Impactt.Domain.Entities;
using Impactt.Service.DTOs;
using System.Linq.Expressions;

namespace Impactt.Service.Interfaces
{
    public interface IUserService
    {

        /// <summary>
        /// Register user.
        /// </summary>
        Task<UserDTO> CreateAsync(UserDTO userDTO);

        /// <summary>
        /// Update user.
        /// </summary>
        Task<UserDTO> UpdateAsync(long id, UserDTO userDTO);

        /// <summary>
        /// Delete user.
        /// </summary>
        Task DeleteAsync(long id);

        /// <summary>
        /// Get user.
        /// </summary>
        Task<UserDTO> GetAsync(Expression<Func<User, bool>> expression);

        /// <summary>
        /// Get all users with pagination.
        /// </summary>
        Task<UserDTO[]> GetAllAsync(int pageIndex, int pageSize, Expression<Func<User, bool>> expression = null);
    }
}
