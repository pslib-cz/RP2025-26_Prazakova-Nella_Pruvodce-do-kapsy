using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{

    public class Floor
    {
        [Key]
        public int FloorId { get; set; }
        public int BuildingId { get; set; }
        public int FloorNumber { get; set; }
    }
}