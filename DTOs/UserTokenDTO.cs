namespace ApiCatalogoController.DTOs
{
    public class UserTokenDTO
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
