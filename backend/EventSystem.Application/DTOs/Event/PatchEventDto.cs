using EventSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.DTOs.Event
{
    public class PatchEventDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public int? Capacity { get; set; }
        public EventType? Type { get; set; }
    }
}
