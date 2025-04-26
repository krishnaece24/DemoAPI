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
        public void GetStories_ShouldReturnOkResult_WithStories()
        {
            // Arrange
            var stories = new List<Story> { new Story { Title = "Test Story", Url = "http://test.com" } };
            _serviceMock.Setup(s => s.GetTopStoriesAsync()).Returns(stories);

            // Act
            var result = _controller.GetStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
