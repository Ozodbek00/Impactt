using AutoMapper;
using AutoMapper.QueryableExtensions;
using Impactt.Data.Interfaces;
using Impactt.Domain.Entities;
using Impactt.Service.DTOs;
using Impactt.Service.Exceptions;
using Impactt.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Impactt.Service.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> repository;
        private readonly IMapper mapper;

        public RoomService(IRepository<Room> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Create room.
        /// </summary>
        public async Task<RoomDTO> CreateAsync(RoomDTO roomDTO)
        {
            var room = await repository.GetAsync(expression: s =>
                  s.Name.Equals(roomDTO.Name));

            if (room is not null)
                throw new ImpacttException(400, "Branch with this name exists");

            Room mappedRoom = mapper.Map<Room>(roomDTO);
            mappedRoom.CreatedAt = DateTime.UtcNow;

            await repository.AddAsync(mappedRoom);

            return roomDTO;
        }

        /// <summary>
        /// Update room.
        /// </summary>
        public async Task<RoomDTO> UpdateAsync(long id, RoomDTO roomDTO)
        {
            var branch = await repository.GetAsync(expression: s => s.Id == id);

            if (branch is null)
                throw new ImpacttException(404, "Branch with this name does not exist");

            Room mappedRoom = mapper.Map<Room>(roomDTO);
            mappedRoom.CreatedAt = branch.CreatedAt;
            mappedRoom.UpdatedAt = DateTime.UtcNow;

            await repository.UpdateAsync(mappedRoom);

            return roomDTO;
        }

        /// <summary>
        /// Delete room.
        /// </summary>
        public async Task DeleteAsync(long id)
        {
            var room = await repository.GetAsync(expression: s => s.Id == id);

            if (room == null)
                throw new ImpacttException(404, "Branch not found");

            await repository.DeleteAsync(room);
        }

        /// <summary>
        /// Get room by expression.
        /// </summary>
        public async Task<RoomDTO> GetAsync(Expression<Func<Room, bool>> expression)
        {
            var room = await repository.GetAsync(expression);

            if (room == null)
                throw new ImpacttException(404, "Branch not found");

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
