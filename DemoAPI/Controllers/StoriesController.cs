using DemoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        /// <summary>
        /// Get New Stories from Hacker API based on pagesize
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNewStories")]
        public async Task<IActionResult> GetNewStories(int page, int pagesize)
        {
            var stories = await _storyService.GetNewStoriesAsync(page, pagesize);
            return Ok(stories);
        }
    }
}
