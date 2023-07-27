using Microservice.Models;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microservice.Redis
{
    public class RedisDatabase : IRedisDatabase
    {
        private readonly IDistributedCache distributedCache;

        public RedisDatabase(IDistributedCache _distributedCache)
        {
            distributedCache = _distributedCache;
        }

        public async Task AddRedis(string cacheKey, User user)
        {
            if (user == null)
            {
                return;
            }

            var serializerResponse = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
            await distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            }); ;

        }

        public async Task<string> GetRedis(string cacheKey)
        {
            var cacheResponse = await distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? "NotFound" : cacheResponse;
        }
    }
}
