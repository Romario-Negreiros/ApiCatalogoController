namespace ApiCatalogoController.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public ICollection<ProductDTO>? Products { get; set; }
    }
}
