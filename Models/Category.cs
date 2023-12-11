using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ApiCatalogoController.Models
{
    public class Category
    {
        public Category()
        {
            Products = new Collection<Product>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
