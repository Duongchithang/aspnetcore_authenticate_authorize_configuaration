using Microservice.Models;

namespace Microservice.Redis
{
    public interface IRedisDatabase
    {
        public Task AddRedis(string cacheKey, User user);
        public Task<string> GetRedis(string cacheKey);
    }
}
