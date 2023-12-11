using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCatalogoController.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "O campo 'nome' não pode estar vázio!")]
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = "O campo 'descrição' não pode estar vázio!")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "O campo 'preço' não pode estar vázio!")]
        public decimal Price { get; set; } = default!;
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "O campo 'estoque' não pode estar vázio!")]
        public int Stock { get; set; } = default!;
        [Required(ErrorMessage = "A data de registro não foi enviada!")]
        public DateTime RegistrationDate { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
