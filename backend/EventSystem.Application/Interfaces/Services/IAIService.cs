using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface IAIService
    {
        Task<string> GetResponseAsync(string userMessage, string snapshot, CancellationToken cancellationToken);
    }
}