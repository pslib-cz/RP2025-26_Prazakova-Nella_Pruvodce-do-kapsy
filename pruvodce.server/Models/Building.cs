using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
    }
}