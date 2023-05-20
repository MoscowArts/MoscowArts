using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoscowArts.Entities;
using MoscowArts.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MoscowArts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository usersService, IConfiguration configuration)
        {
            _userService = usersService;
            _configuration = configuration;
        }

        [HttpPost("cheakEmail")]
        public async Task<ActionResult<User>> Register(string Email)
        {
            var User = (await _userService.FindAsync(u => u.Email == Email)).FirstOrDefault();
            if(User != null)
                return BadRequest();

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = (await _userService.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (user == null)
                return BadRequest();

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Email = request.Email;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Patronymic = request.Patronymic;
            user.Phone = request.Phone;
            user.Age = request.Age;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            await _userService.AddAsync(user);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string Email, string Password)
        {
            var user = (await _userService.FindAsync(u => u.Email == Email)).FirstOrDefault();

            if (user == null)
            {
                return new UnauthorizedResult();
            }
            if (!VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
            {
                return new UnauthorizedResult();
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();

            //var cookiesOptions = new CookieOptions()
            //{
            //    HttpOnly = true,
            //    Expires = refreshToken.Expires
            //};
            //Response.Cookies.Append("refreshToken", refreshToken.Token, cookiesOptions);
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
            return Ok(token);
        }

        private List<Claim> GetClaimsAsync(User user)
        {
            //string allRoles = string.Join(",", _roleManager.Roles.ToList());
            List<Claim> claim = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
            };
            return claim;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(1)
            };
        }

        private string CreateToken(User user)
        {
            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var keyBytes = new byte[64]; // 64 байта = 512 бит
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }
            var key = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: GetClaimsAsync(user),
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
