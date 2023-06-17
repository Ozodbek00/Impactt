using Impactt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Impactt.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<UserRoomBook> UsersRoomsBooks { get; set;}
    }
}
