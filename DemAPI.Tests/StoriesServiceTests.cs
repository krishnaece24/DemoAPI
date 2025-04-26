using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using DemoAPI.Services;
using DemoAPI.Models;

namespace DemoAPI.Tests
{
    public class StoriesServiceTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly StoryService _service;
        private readonly Mock<ILogger<StoryService>> _loggerMock;

        public StoriesServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _loggerMock = new Mock<ILogger<StoryService>>();
            _service = new StoryService(_memoryCache, _loggerMock.Object);
        }

        [Fact]
        public void GetTopStoriesAsync_ShouldReturnStories_WhenCacheIsEmpty()
        {
            // Arrange
            var expectedStories = new List<Story> { new Story { Title = "Story Number 1", Url = "Story URL 1" } };

            // Act
            var result = _service.GetTopStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Story Number 1", result.First().Title);
        }

        [Fact]
        public void GetTopStoriesAsync_ShouldReturnStories_FromCache()
        {
            // Arrange
            var cachedStories = new List<Story> { new Story { Title = "Story Number 1", Url = "Story URL 1" } };
            _memoryCache.Set("TopStories", cachedStories);

            // Act
            var result = _service.GetTopStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Story Number 1", result.First().Title);
        }
    }
}
