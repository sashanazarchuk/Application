using EventSystem.Application.DTOs.Tag;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetOrCreateTagsAsync(List<string> tagNames, CancellationToken cancellationToken);
        Task UpdateEventTagsAsync(Event existingEvent, List<string> newTagNames, CancellationToken cancellationToken);
    }
}