﻿using Impactt.Domain.Enums;

namespace Impactt.Service.DTOs
{
    public sealed class RoomDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public RoomType Type { get; set; }

        public byte Capacity { get; set; }
    }
}
