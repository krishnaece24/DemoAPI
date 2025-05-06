using Moq;
using Microsoft.AspNetCore.Mvc;
using DemoAPI.Services;
using DemoAPI.Controllers;
using DemoAPI.Models;

namespace DemoAPI.Tests
{
    public class StoriesControllerTests
    {
        private readonly Mock<IStoryService> _serviceMock;
        private readonly StoriesController _controller;

        public StoriesControllerTests()
        {
            _serviceMock = new Mock<IStoryService>();
            _controller = new StoriesController(_serviceMock.Object);
        }

        [Fact]
        public void GetNewStories_Result()
        {
            // Arrange
            var stories = new List<HackerNewsStory> { new HackerNewsStory { Title = "Test Story", Url = "http://test.com" } };
            _serviceMock.Setup(s => s.GetNewStoriesAsync()).Returns(Task.FromResult(stories));

            // Act
            var result = _controller.GetNewStories();

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
        }
    }
}
