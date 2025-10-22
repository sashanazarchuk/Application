using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.DTOs.Users
{
    public class ParticipantDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
