using ApiCatalogoController.DTOs;

namespace ApiCatalogoController.Services
{
    public interface IJwtService
    {
        UserTokenDTO GenerateJwt(string userEmail);
    }
}
