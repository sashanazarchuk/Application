using EventSystem.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.AI.GetAIResponse
{
    internal class GetAIResponseQueryHandler : IRequestHandler<GetAIResponseQuery, string>
    {
        private readonly IAIService _aiService;
        private readonly IUserSnapshotService _userSnapshotService;
        private readonly ILogger<GetAIResponseQueryHandler> _logger;
        public GetAIResponseQueryHandler(IAIService aiService, IUserSnapshotService userSnapshotService, ILogger<GetAIResponseQueryHandler> logger)
        {
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
            _userSnapshotService = userSnapshotService ?? throw new ArgumentNullException(nameof(userSnapshotService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(GetAIResponseQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting AI response handling for user message: {Message}", request.Dto.MessageToAI);

            var snapshotDto = await _userSnapshotService.GetUserSnapshotAsync(cancellationToken);
            _logger.LogDebug("User snapshot retrieved: {@Snapshot}", snapshotDto);

            var snapshotJson = JsonSerializer.Serialize(snapshotDto);
            
            var aiResponse = await _aiService.GetResponseAsync(
                userMessage: request.Dto.MessageToAI,
                snapshot: snapshotJson,
                cancellationToken: cancellationToken
            );

            _logger.LogInformation("AI response received successfully.");
            return aiResponse;
        }
    }
}