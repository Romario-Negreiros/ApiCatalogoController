using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext ctx;

        public ProductsController(AppDbContext _ctx)
        {
            ctx = _ctx;
        }
        private ObjectResult HandleServerError(Exception ex)
        {
            Console.Out.WriteLine(ex.ToString());
            return Problem("Erro ao processar a requisição!", null, StatusCodes.Status500InternalServerError);
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                // Limitar a quantidade de produtos retornados por requisição
                List<Product> products = await ctx.Products.AsNoTracking().Include(p => p.Category).ToListAsync();
                if (!products.Any())
                {
                    return Ok("Nenhum produto existente!");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpGet("id: int")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Product? product = await ctx.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Produto não encontrada!");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(Product product)
        {
            if (product.Name == null)
            {
                return BadRequest("O campo 'nome' não pode estar vazio!");
            }
            if (product.Description == null)
            {
                return BadRequest("O campo 'descrição' não pode estar vazio!");
            }
            try
            {
                ctx.Products.Add(product);
                await ctx.SaveChangesAsync();
                return Created($"/products/{product.ProductId}", product);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPut("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("O id do produto não corresponde com id passado como parametro!");
            }
            if (product.Name == null)
            {
                return BadRequest("O campo 'nome' não pode estar vazio!");
            }
            if (product.Description == null)
            {
                return BadRequest("O campo 'descrição' não pode estar vazio!");
            }
            try
            {
                Product? productInDb = await ctx.Products.FindAsync(id);
                if (productInDb == null)
                {
                    return NotFound("O produto que deseja atualizar não foi encontrado!");
                }
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.ImageUrl = product.ImageUrl;
                productInDb.RegistrationDate = product.RegistrationDate;
                productInDb.Stock = product.Stock;
                productInDb.CategoryId = product.CategoryId;
                await ctx.SaveChangesAsync();
                return Ok(productInDb);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpDelete("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Product? produto = await ctx.Products.FindAsync(id);
                if (produto == null)
                {
                    return NotFound("O produto que deseja deletar não existe!");
                }
                ctx.Products.Remove(produto);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
    }
}
