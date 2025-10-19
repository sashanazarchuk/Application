using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.DTOs.Event
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public int? Capacity { get; set; }
        public int CurrentParticipantsCount { get; set; }
        public bool IsFull => Capacity.HasValue && CurrentParticipantsCount >= Capacity;
    }
}