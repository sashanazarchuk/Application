using EventSystem.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Services
{
    internal class PromptReaderService : IPromptReaderService
    {
        private readonly string _promptsDirectory;

        public PromptReaderService()
        {
            _promptsDirectory = Path.Combine(AppContext.BaseDirectory, "Prompts");
        }

        public async Task<string> GetSystemPromptAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(_promptsDirectory, "system_prompt.txt");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Prompt file not found: {path}");

            return await File.ReadAllTextAsync(path, cancellationToken);
        }
    }
}