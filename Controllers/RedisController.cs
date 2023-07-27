//using Microservice.Models;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Distributed;

//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

//namespace Microservice.Controllers
//{
//    [Route("/api/[action]")]
//    [ApiController]
//    public class RedisController : ControllerBase
//    {
//        private readonly IDistributedCache distributedCache;

//        public RedisController(IDistributedCache _distributedCache)
//        {
//            distributedCache = _distributedCache;
//        }
//        [HttpGet]
//        public async Task<string> GetDataRedis(string cacheKey)
//        {
//            var cacheResponse = await distributedCache.GetStringAsync(cacheKey);
//            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
//        }
//        [HttpPost]
//        public async Task PostDataRedis(string cacheKey, InfoUser _infoUser)
//        {
//            if (_infoUser == null)
//            {
//                return;
//            }

//            var serializerResponse = JsonConvert.SerializeObject(_infoUser, new JsonSerializerSettings()
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver(),
//            });
//            await distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions()
//            {
//                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
//            }); ;
//        }
//    }
//}
