using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using DemoAPI.Services;
using DemoAPI.Models;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DemoAPI.Tests
{
    public class StoriesServiceTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly StoryService _service;
        private readonly Mock<ILogger<StoryService>> _loggerMock;
        private readonly Mock<HttpClient> _httpClient;
        private readonly Mock<IConfiguration> _configuration;

        public StoriesServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _loggerMock = new Mock<ILogger<StoryService>>();
            _httpClient = new Mock<HttpClient>();
            _configuration = new Mock<IConfiguration>();
            
        }

        private HttpClient CreateHttpClientMock(Dictionary<string, string> responses)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Returns<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    var url = request.RequestUri.ToString();
                    var content = responses.ContainsKey(url) ? responses[url] : "{}";
                    return Task.FromResult(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(content)
                    });
                });

            return new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };
        }

        [Fact]
        public async Task GetTopStoriesAsync_ReturnsCorrectCount()
        {
            // Arrange
            var ids = Enumerable.Range(1, 3).ToList();
            var idJson = JsonSerializer.Serialize(ids);

            var storyJson = new Func<int, string>(id =>
              JsonSerializer.Serialize(new
              {
                  id = id,
                  title = $"Title {id}",
                  by = $"User{id}",
                  score = 100 + id,
                  time = 123456 + id,
                  url = $"https://news.ycombinator.com/item?id={id}"
              }));

            var responses = new Dictionary<string, string>
            {
                ["https://hacker-news.firebaseio.com/v0/topstories.json"] = idJson
            };

            foreach (var id in ids)
                responses[$"https://hacker-news.firebaseio.com/v0/item/{id}.json"] = storyJson(id);

            var httpClient = CreateHttpClientMock(responses);

            _configuration.Setup(c => c["HackerNewsApi:TopCount"]).Returns("10");
            _configuration.Setup(c => c["HackerNewsApi:BaseAddress"]).Returns("https://hacker-news.firebaseio.com/v0/");

            var service = new StoryService(_memoryCache, _loggerMock.Object, httpClient, _configuration.Object);

            // Act
            var stories = await service.GetNewStoriesAsync(0,3);

            // Assert
            Assert.Equal(3, stories.Count);
        }
    }
}
