using ApiCatalogoController.Models;
using ApiCatalogoController.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly ConfigurationManager configuration;
        public AuthController(IJwtService _jwtService, ConfigurationManager _configuration)
        {
            jwtService = _jwtService;
            configuration = _configuration;
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user == null)
            {
                return BadRequest("Login inválido!");
            }
            if (user.Name == "Romario" && user.Password == "123")
            {
                var token = jwtService.GenerateJwt(configuration["Jwt:Key"], configuration["Jwt:Issuer"], configuration["Jwt:Audience"], user);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Login inválido");
            }
        }
    }
}
