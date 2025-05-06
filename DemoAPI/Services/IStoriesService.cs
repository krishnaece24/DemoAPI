using DemoAPI.Models;

namespace DemoAPI.Services
{
    public interface IStoryService
    {
        Task<List<HackerNewsStory>> GetNewStoriesAsync(int offset, int pagesize);
    }

}
