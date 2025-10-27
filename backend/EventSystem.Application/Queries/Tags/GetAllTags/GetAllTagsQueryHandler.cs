using AutoMapper;
using EventSystem.Application.DTOs.Tag;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Tags.GetAllTags
{
    internal class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IEnumerable<TagDto>>
    {
        private readonly ITagRepository _tagRepository;
        private ILogger<GetAllTagsQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllTagsQueryHandler(ITagRepository tagRepository, ILogger<GetAllTagsQueryHandler> logger, IMapper mapper)
        {
            _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all tags");
            var tags = await _tagRepository.GetAllAsync(cancellationToken);

            _logger.LogInformation("Returning {TagCount} tag DTOs", tags.Count());
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }
    }
}