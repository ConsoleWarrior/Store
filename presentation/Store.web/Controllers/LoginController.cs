using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Authentication;
using Store.web.App;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.web.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IConfiguration _config;

        public LoginController(LoginService loginService, IConfiguration config)
        {
            _loginService = loginService;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Logining(string login, string pass)
        {
            try
            {
                var model = _loginService.TryLogining(login, pass);
                if (model.Result)
                {
                    var token = GenerateToken(model);
                    var response = new LoginResponseWithToken() { Name = login, Token = token, Result = model.Result, Role = model.Role };
                    return View(response);
                }
                else return View(new LoginResponseWithToken() { Result = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public IActionResult RegistrationButton()
        {
            return View("Registration");
        }

        [HttpPost]
        public IActionResult Registration(string name, string email, string phone, string password, string passwordConfirmation)
        {
            if (!password.Equals(passwordConfirmation))
            {
                return View("RegistrationResult", false);
            }
            return View("RegistrationResult", _loginService.TryRegistration(name, email, phone, password));
        }

        //[AllowAnonymous]
        //[HttpPost("AddUser")]
        //public ActionResult<int> AddUser(UserDto user)
        //{
        //    try
        //    {
        //        return Ok(_repository.AddUser(user));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(409, ex.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost("Login")]
        //public ActionResult Login([FromBody] LoginDto login)
        //{
        //    try
        //    {
        //        var roleId = _repository.CheckUser(login);
        //        var user = new UserDto
        //        {
        //            Name = login.Name,
        //            Password = login.Password,
        //            Role = roleId
        //        };
        //        var token = GenerateToken(user);
        //        return Ok(token);
        //    }
        //    catch (Exception ex)
        //    { return StatusCode(500, ex.Message); }
        //}

        private string GenerateToken(LoginResponse user)
        {
            // = new RsaSecurityKey(RSAExtensions.GetPrivateKey());
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
