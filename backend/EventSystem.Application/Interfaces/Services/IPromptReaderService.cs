using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface IPromptReaderService
    {
        Task<string> GetSystemPromptAsync(CancellationToken cancellationToken);
    }
}
