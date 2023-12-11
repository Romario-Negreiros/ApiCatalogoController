using ApiCatalogoController.Models;

namespace ApiCatalogoController.Services
{
    public interface IJwtService
    {
        string GenerateJwt(string key, string issuer, string audience, User user);
    }
}
