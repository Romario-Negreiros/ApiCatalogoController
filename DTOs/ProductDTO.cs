namespace ApiCatalogoController.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public int? Stock { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
