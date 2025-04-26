using DemoAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DemoAPI.Services
{
    public class StoryService : IStoryService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<StoryService> _logger;

        public StoryService(IMemoryCache cache, ILogger<StoryService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public List<Story> GetTopStoriesAsync()
        {
            if (_cache.TryGetValue("TopStories", out List<Story> stories))
                if (stories != null && stories.Count > 0)
                    return stories;

            stories = new List<Story>();

            for (int i = 1; i <= 200; i++)
            {
                Story story = new Story();
                story.Title = "Story Number " + i;
                story.Url = "Story URL " + i;
                stories.Add(story);
            }
            _cache.Set("TopStories", stories, TimeSpan.FromMinutes(10));
            return stories;
        }
    }
}
