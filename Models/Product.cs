using System.Text.Json.Serialization;

namespace ApiCatalogoController.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public int Stock { get; set; } = default!;
        public DateTime RegistrationDate { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
