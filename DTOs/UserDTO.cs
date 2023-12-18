namespace ApiCatalogoController.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
