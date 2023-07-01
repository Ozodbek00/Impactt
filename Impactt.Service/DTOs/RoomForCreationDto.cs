using Impactt.Domain.Enums;

namespace Impactt.Service.DTOs
{
    public sealed class RoomForCreationDto
    {
        public string Name { get; set; }

        public RoomType Type { get; set; }

        public byte Capacity { get; set; }
    }
}
