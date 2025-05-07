using DemoAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DemoAPI.Services
{
    public class StoryService : IStoryService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<StoryService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StoryService(IMemoryCache cache, ILogger<StoryService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _cache = cache;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<HackerNewsStory>> GetNewStoriesAsync(int page, int pagesize)
        {
            _cache.TryGetValue("NewStories", out List<int>? ids);

            string? baseUrl = _configuration["HackerNewsApi:BaseAddress"];
            _httpClient.BaseAddress = new Uri(baseUrl?? "");

            if (ids == null || ids.Count == 0)
            {
                var response = await _httpClient.GetStringAsync("topstories.json");
                ids = JsonSerializer.Deserialize<List<int>>(response);

                _cache.Set("NewStories", ids, TimeSpan.FromMinutes(10));
            }

            var topIds = ids?.Skip((page -1) * pagesize).Take(pagesize).ToList();
            var stories = new List<HackerNewsStory>();

            for (int i = 0; i < topIds?.Count(); i++)
            {
                HackerNewsStory story = new HackerNewsStory();
                var storyJson = await _httpClient.GetStringAsync("item/" + topIds[i] + ".json");
                stories.Add(JsonSerializer.Deserialize<HackerNewsStory>(storyJson) ?? new HackerNewsStory());
            }
            return stories;
        }
    }
}
