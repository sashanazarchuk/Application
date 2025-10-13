using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Domain.Entities
{
    public class User:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        
        public List<Event> AdminEvents { get; set; } = new List<Event>();
        public List<Participant> Participations { get; set; } = new List<Participant>();
    }
}