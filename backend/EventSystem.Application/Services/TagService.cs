using EventSystem.Application.DTOs.Tag;
using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventSystem.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
        }
        public async Task<List<Tag>> GetOrCreateTagsAsync(List<string> tagNames, CancellationToken cancellationToken)
        {
            if (tagNames.Count > 5)
                throw new ForbiddenException("Maximum 5 tags allowed.");

            var normalizedNames = tagNames
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => char.ToUpper(t[0]) + t.Substring(1).ToLower())
                .Distinct()
                .ToList();

            var existingTags = await _tagRepository.GetByNamesAsync(normalizedNames, cancellationToken);
            var existingTagsDict = existingTags.ToDictionary(t => t.Name, t => t);

            var newTags = normalizedNames
                .Where(name => !existingTagsDict.ContainsKey(name))
                .Select(name => new Tag { Name = name })
                .ToList();

            if (newTags.Any())
                await _tagRepository.AddRangeAsync(newTags, cancellationToken);

            foreach (var t in newTags)
                existingTagsDict[t.Name] = t;

            return existingTagsDict.Values.ToList();
        }


        public async Task UpdateEventTagsAsync(Event existingEvent, List<string> newTagNames, CancellationToken cancellationToken)
        {
            if (newTagNames == null)
                return;
           
            var newTags = await GetOrCreateTagsAsync(newTagNames, cancellationToken);

            var tagsToRemove = existingEvent.EventTags
                .Where(existTag => !newTags.Any(newTag => newTag.Id == existTag.TagId))
                .ToList();
            
            foreach (var existTag in tagsToRemove)
                existingEvent.EventTags.Remove(existTag);

           
            var tagsToAdd = newTags
                .Where(newTag => !existingEvent.EventTags.Any(existTag => existTag.TagId == newTag.Id))
                .ToList();

            foreach (var newTag in tagsToAdd)
                existingEvent.EventTags.Add(new EventTag { EventId = existingEvent.Id, TagId = newTag.Id });
        }
    }
}
