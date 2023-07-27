using AutoMapper;

using Microservice.JWT;
using Microservice.Models;
using Microservice.Redis;
using Microservice.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [Route("/api/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRedisDatabase redisDatabase;
        private readonly redisContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly JsonWebToken jwt;

        public UserController(IRedisDatabase _redisDatabase, redisContext _context, IMapper _mapper, IConfiguration _configuration, JsonWebToken _jwt)
        {
            redisDatabase = _redisDatabase;
            context = _context;
            mapper = _mapper;
            configuration = _configuration;
            jwt = _jwt;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetUser(int Id)
        //{

        //    var getUserRedis = redisDatabase.GetRedis($"user{Id}");
        //    string result = await getUserRedis;

        //    if (result == "NotFound")
        //    {
        //        var getUser = context.Users.FirstOrDefault(v => v.Id == Id);


        //        var addUserToRedis = redisDatabase.AddRedis($"user{Id}", getUser);
        //    }
        //    var userViewModel = mapper.Map<UserViewModel>(result);
        //    Log.Information($"result => {result}");
        //    return Ok(new
        //    {
        //        result,
        //        userViewModel
        //    });
        //}
        [HttpPost]
        public IActionResult Login(LoginViewModel loginView)
        {
            var usernameLogin = loginView.username;
            var passwordLogin = loginView.password;
            var getInfoUser = context.User.FirstOrDefault(v => v.Username == usernameLogin);
            var getRoles = from s in context.Roles where s.IdUser == getInfoUser.Id select s.RoleName;
            if (getInfoUser == null)
            {
                return NotFound("User Not Found");
            }
            if (getInfoUser.Password != passwordLogin)
            {
                return BadRequest("Password is correct");
            }
            var AccessTokenValue = jwt.GenerateAccessToken(getInfoUser, getRoles);
            var RefreshTokenValue = jwt.GenerateRefreshToken(getInfoUser, getRoles);
            CookieOptions options = new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append("RefreshToken", RefreshTokenValue, options);
            var userID = getInfoUser.Id;
            var InfoToken = new TokenTable
            {
                IdToken = Guid.NewGuid().GetHashCode(),
                IdUser = userID,
                AccessToken = AccessTokenValue,
                RefreshToken = RefreshTokenValue,
                CreateAt = DateTime.UtcNow.AddMinutes(1),
                IdUserNavigation = null

            };
            context.TokenTable.Add(InfoToken);
            context.SaveChanges();
            return Ok(new
            {
                InfoToken,
                getInfoUser,
                AccessToken = AccessTokenValue,
                RefreshToken = RefreshTokenValue
            });

        }
        [HttpPost]
        public IActionResult RefreshToken(int IdToken)
        {
            var RefreshTokenCookie = Request.Cookies["RefreshToken"];
            var RefreshTokenDB = context.TokenTable.FirstOrDefault((v) => v.IdToken == IdToken);
            if (!RefreshTokenDB.RefreshToken.Equals(RefreshTokenCookie))
            {
                return Unauthorized();
            }
            else if (RefreshTokenDB.CreateAt < DateTime.UtcNow)
            {
                return BadRequest("Token expried");
            }


            //var newToken = jwt.GenerateAccessToken(getInfoUser, )
            return Ok(new
            {
                mesage = "gọi token được"
            });
        }

    }
}
