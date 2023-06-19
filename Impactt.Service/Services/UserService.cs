using AutoMapper;
using AutoMapper.QueryableExtensions;
using Impactt.Data.Interfaces;
using Impactt.Domain.Entities;
using Impactt.Service.DTOs;
using Impactt.Service.Exceptions;
using Impactt.Service.Extensions;
using Impactt.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Impactt.Service.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        public async Task<UserDTO> CreateAsync(UserForCreationDTO userDTO)
        {
            var user = await repository.GetAsync(expression: s => s.Email.Equals(userDTO.Email));

            if (user is not null)
                throw new ImpacttException(400, "User with this email address exists");

            var validUser = await repository.GetAsync(expression: s =>
                                      s.Email.Equals(userDTO.Email) ||
                                      s.Password.Equals(userDTO.Password.Encrypt()));

            if (validUser is not null)
                throw new ImpacttException(400, "Invalid password or email address");

            User mappedUser = mapper.Map<User>(userDTO);
            mappedUser.CreatedAt = DateTime.UtcNow;
            mappedUser.Password = mappedUser.Password.Encrypt();

            await repository.AddAsync(mappedUser);

            return mapper.Map<UserDTO>(mappedUser);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        public async Task DeleteAsync(long id)
        {
            var user = await repository.GetAsync(expression: s => s.Id == id);

            if (user == null)
                throw new ImpacttException(404, "User not found");

            await repository.DeleteAsync(user);
        }

        /// <summary>
        /// Get all users with pagination.
        /// </summary>
        public async Task<UserDTO[]> GetAllAsync(int pageIndex, int pageSize, Expression<Func<User, bool>> expression = null)
        {
            return await repository.GetAll(pageIndex, pageSize, expression)
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).ToArrayAsync();
        }

        /// <summary>
        /// Get user.
        /// </summary>
        public async Task<UserDTO> GetAsync(Expression<Func<User, bool>> expression)
        {
            var User = await repository.GetAsync(expression);

            if (User is null)
                throw new ImpacttException(404, "User not found");

            return mapper.Map<UserDTO>(User);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        public async Task<UserDTO> UpdateAsync(long id, UserForCreationDTO userDTO)
        {
            var user = await repository.GetAsync(expression: s => s.Id == id);

            if (user is null)
                throw new ImpacttException(404, "User not found");

            var passUser = await repository.GetAsync(expression: s =>
                                      (s.Email.Equals(userDTO.Email) ||
                                      s.Password.Equals(userDTO.Password.Encrypt())) &&
                                      s.Id != id);

            if (passUser != null)
                throw new ImpacttException(400, "Invalid password or email address");

            User mappedUser = mapper.Map(userDTO, user);
            mappedUser.CreatedAt = user.CreatedAt;
            mappedUser.UpdatedAt = DateTime.UtcNow;
            mappedUser.Password = mappedUser.Password.Encrypt();

            await repository.UpdateAsync(mappedUser);

            return mapper.Map<UserDTO>(mappedUser);
        }
    }
}
