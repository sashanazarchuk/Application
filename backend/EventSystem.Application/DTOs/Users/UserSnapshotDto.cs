using EventSystem.Application.DTOs.Event;
using EventSystem.Application.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.DTOs.Users
{
    public class UserSnapshotDto
    {
        public UserDto User { get; set; } = null!;
        public List<EventDto> AdminEvents { get; set; } = new();
        public List<EventDto> JoinedEvents { get; set; } = new();
        public List<EventDto> PublicEvents { get; set; } = new();
        public List<TagDto> AllTags { get; set; } = new();
    }
}
