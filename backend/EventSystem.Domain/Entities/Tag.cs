using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Domain.Entities
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<EventTag> EventTags { get; set; } = new();
    }
}