using ApiCatalogoController.Models;
using ApiCatalogoController.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork uof;

        public ProductsController(IUnitOfWork _uof)
        {
            uof = _uof;
        }
        private ObjectResult HandleServerError(Exception ex)
        {
            Console.Out.WriteLine(ex.ToString());
            return Problem("Erro ao processar a requisição!", null, StatusCodes.Status500InternalServerError);
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                // Limitar a quantidade de produtos retornados por requisição
                List<Product> products = await uof.ProductRepository.Get().Include(p => p.Category).ToListAsync();
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
        [HttpGet("{id:int:min(1)}")] // Restrição: id > 0
        public async Task<ActionResult<Product>> Get(int id)
        {
            try
            {
                Product? product = await uof.ProductRepository.GetById(p => p.ProductId == id);
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
        public async Task<ActionResult<Product>> Post(Product product)
        {
            try
            {
                await uof.ProductRepository.Add(product);
                await uof.Commit();
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
            try
            {
                uof.ProductRepository.Update(product);
                await uof.Commit();
                return Ok();
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
                Product? produto = await uof.ProductRepository.GetById(p => p.ProductId == id);
                if (produto == null)
                {
                    return NotFound("O produto que deseja deletar não existe!");
                }
                uof.ProductRepository.Delete(produto);
                await uof.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
    }
}
