using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.DTOs.Auth
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}