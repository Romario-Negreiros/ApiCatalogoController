using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "O campo 'nome' não pode estar vázio!")]
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = "O campo 'descrição' não pode estar vázio!")]
        public string Description { get; set; } = default!;
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
