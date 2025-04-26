using DemoAPI.Models;

namespace DemoAPI.Services
{
    public interface IStoryService
    {
        List<Story> GetTopStoriesAsync();
    }

}
