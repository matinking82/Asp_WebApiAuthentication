using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApiAuthentication.DatabaseContext;
using WebApiAuthentication.Dtos;
using WebApiAuthentication.Utils;

namespace WebApiAuthentication.Controllers
{
    [ApiController]
    [Route("users")]
    public class AuthenticationController : ControllerBase
    {
        private readonly MyDatabaseContext _context;
        private readonly HttpHelper _httpHelper;
        public AuthenticationController(MyDatabaseContext context, HttpHelper httpHelper)
        {
            _context = context;
            _httpHelper = httpHelper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.GetId();
            var user = await _context.Users.FindAsync(userId);

            return Ok(new
            {
                message = "you are logged in",
                user
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == requestDto.Username);

            if (user != null)
            {
                return BadRequest(new
                {
                    message = "Username already exists"
                });
            }

            var created = await _context.Users.AddAsync(new()
            {
                Username = requestDto.Username,
                PasswordHash = requestDto.Password.ToSha256Async().Result
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "user registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == requestDto.Username);

            if (user == null)
            {
                return BadRequest(new
                {
                    message = "Invalid username or password"
                });
            }

            if (user.PasswordHash != requestDto.Password.ToSha256Async().Result)
            {
                return BadRequest(new
                {
                    message = "Invalid username or password"
                });
            }

            var token = _httpHelper.GenerateJsonWebToken(user.Id);

            return Ok(new
            {
                message = "Login Successful",
                token,
                token_type = "bearer"
            });
        }
    }
}
