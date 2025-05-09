﻿using Xunit;
using Microsoft.Extensions.Caching.Memory;
using DemoAPI.Models;

namespace DemoAPI.Tests
{
    public class CachingTests
    {
        private readonly IMemoryCache _memoryCache;

        public CachingTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void Cache_ShouldStoreAndRetrieveValue()
        {
            var key = "TestKey";
            var story = new HackerNewsStory { title = "Cached Title", url = "http://cached.com" };

            _memoryCache.Set(key, story, TimeSpan.FromMinutes(5));
            var exists = _memoryCache.TryGetValue(key, out HackerNewsStory cachedStory);

            Assert.True(exists);
            Assert.Equal("Cached Title", cachedStory.title);
        }
    }
}
