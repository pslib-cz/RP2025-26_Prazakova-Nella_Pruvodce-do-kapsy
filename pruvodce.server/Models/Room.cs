using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Room
    {
        [Key]
        public string RoomId { get; set; } = string.Empty;
        public int FloorId { get; set; }
    }
}