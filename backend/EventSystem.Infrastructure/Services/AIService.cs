using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
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
        private readonly IMemoryCache _cache;

        public AIService(IOptions<AISettings> aiSettings, HttpClient httpClient, IPromptReaderService promptReaderService, IMemoryCache cache)
        {
            _aiSettings = aiSettings.Value ?? throw new ArgumentNullException(nameof(aiSettings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _promptReaderService = promptReaderService ?? throw new ArgumentNullException(nameof(promptReaderService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> GetResponseAsync(string userMessage, string snapshot, CancellationToken cancellationToken)
        {
            var cacheKey = GenerateCacheKey(userMessage, snapshot);

            // Try to get from cache first.
            if (_cache.TryGetValue(cacheKey, out string? cachedResponse) && cachedResponse is not null)
            {
                return cachedResponse;
            }

            // If not in cache, execute the call and cache the result for next time.
            return await GetAndCacheAIResponseAsync(cacheKey, userMessage, snapshot, cancellationToken);
        }

        private async Task<string> GetAndCacheAIResponseAsync(string cacheKey, string userMessage, string snapshot, CancellationToken cancellationToken)
        {
            var systemPrompt = await _promptReaderService.GetSystemPromptAsync(cancellationToken);
            var request = BuildAIServiceRequest(userMessage, snapshot, systemPrompt);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken) ?? "No error content";
                throw new BusinessException($"AI service request failed with status code {response.StatusCode}. Response: {errorContent}");
            }

            var aiResponse = await ParseAIServiceResponse(response, cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, aiResponse, cacheEntryOptions);

            return aiResponse;
        }

        private HttpRequestMessage BuildAIServiceRequest(string userMessage, string snapshot, string systemPrompt)
        {
            var snapshotObject = JsonSerializer.Deserialize<object>(snapshot);
            var requestBody = new
            {
                message = userMessage,
                snapshot = snapshotObject,
                system_prompt = systemPrompt
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _aiSettings.AIServiceUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            return request;
        }

        private async Task<string> ParseAIServiceResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
        }

        private string GenerateCacheKey(string userMessage, string snapshot)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedString = $"{userMessage}|{snapshot}";
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}