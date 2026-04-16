using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Floor
    {
        [Key]
        public int FloorId { get; set; }
        public required string Name { get; set; }
        public required string SvgOutline { get; set; }

        public List<Room> Rooms { get; set; } = new List<Room>();

        public int BuildingId { get; set; }
        public Building? Building { get; set; }
    }
}