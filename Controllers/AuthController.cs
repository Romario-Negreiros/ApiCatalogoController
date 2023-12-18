using ApiCatalogoController.DTOs;
using ApiCatalogoController.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IJwtService jwtService;
        public AuthController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, IJwtService _jwtService, ConfigurationManager _configuration)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            jwtService = _jwtService;
            configuration = _configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Authorization Controller :: Acessado em :" + DateTime.Now.ToLongDateString();
        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return Ok(jwtService.GenerateJwt(user.Email));
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Erro ao tentar realizar o login!");
                return BadRequest(ModelState);
            }
            return Ok(jwtService.GenerateJwt(userInfo.Email));
        }
    }
}
