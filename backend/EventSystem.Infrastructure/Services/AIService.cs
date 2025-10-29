using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Services
{
    internal class AIService : IAIService
    {
        private readonly AISettings _aiSettings;
        private readonly HttpClient _httpClient;
        private readonly IPromptReaderService _promptReaderService;

        public AIService(IOptions<AISettings> aiSettings, HttpClient httpClient, IPromptReaderService promptReaderService)
        {
            _aiSettings = aiSettings.Value ?? throw new ArgumentNullException(nameof(aiSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _promptReaderService = promptReaderService ?? throw new ArgumentNullException(nameof(promptReaderService));
        }
        public async Task<string> GetResponseAsync(string userMessage, string snapshot, CancellationToken cancellationToken)
        {
            var systemPrompt = await _promptReaderService.GetSystemPromptAsync(cancellationToken);

            var request = BuildAIRequest(userMessage, snapshot, systemPrompt);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
                throw new BusinessException($"AI API request failed with status code {response.StatusCode}");

            return await ParseAIResponse(response, cancellationToken);
        }

        private HttpRequestMessage BuildAIRequest(string userMessage, string snapshot, string systemPrompt)
        {
            var requestBody = new
            {
                model = _aiSettings.Model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage + "\nSnapshot:\n" + snapshot }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _aiSettings.ApiUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Bearer {_aiSettings.ApiKey}");
            return request;
        }

        private async Task<string> ParseAIResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            
            using var document = JsonDocument.Parse(json);
            
            return document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? string.Empty;
        }
    }
}