using EventSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Domain.Entities
{
    public class Event:BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public int? Capacity { get; set; }
        public Guid AdminId { get; set; }
        public User Admin { get; set; } = null!;
        public EventType Type { get; set; } = EventType.Public;
        public List<Participant> Participants { get; set; } = new();

        public List<EventTag> EventTags { get; set; } = new();
    }
}