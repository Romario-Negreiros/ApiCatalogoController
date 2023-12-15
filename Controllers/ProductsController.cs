using ApiCatalogoController.DTOs;
using ApiCatalogoController.Models;
using ApiCatalogoController.Repositories;
using AutoMapper;
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
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork _uof, IMapper _mapper)
        {
            uof = _uof;
            mapper = _mapper;
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
                List<Product> products = await uof.ProductRepository.Get().Include(p => p.Category).ToListAsync();
                if (!products.Any())
                {
                    return Ok("Nenhum produto existente!");
                }
                List<ProductDTO> productsDTO = mapper.Map<List<ProductDTO>>(products);
                return Ok(productsDTO);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpGet("{id:int:min(1)}")] // Restrição: id > 0
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Product? product = await uof.ProductRepository.GetById(p => p.ProductId == id);
                if (product == null)
                {
                    return NotFound("Produto não encontrado!");
                }
                ProductDTO productDTO = mapper.Map<ProductDTO>(product);
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = mapper.Map<Product>(productDTO);
                product.RegistrationDate = DateTime.Now;
                await uof.ProductRepository.Add(product);
                await uof.Commit();
                ProductDTO updatedProductDTO = mapper.Map<ProductDTO>(product);
                return Created($"/products/{updatedProductDTO.ProductId}", updatedProductDTO);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPut("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return BadRequest("O id do produto não corresponde com id passado como parametro!");
            }
            try
            {
                Product product = mapper.Map<Product>(productDTO);
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
