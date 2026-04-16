using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }

        public List<Floor> Floors { get; set; } = new List<Floor>();
    }
}