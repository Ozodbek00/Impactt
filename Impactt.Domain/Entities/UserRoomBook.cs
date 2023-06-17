namespace Impactt.Domain.Entities
{
    public sealed class UserRoomBook : BaseModel
    {
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long RoomId { get; set; }
        public Room Room { get; set; }
    }
}
