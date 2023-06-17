using Impactt.Domain.Enums;

namespace Impactt.Domain.Entities
{
    public sealed class Room : BaseModel
    {
        public string Name { get; set; }

        public RoomType Type { get; set; }

        public byte Capacity { get; set; }
    }
}
