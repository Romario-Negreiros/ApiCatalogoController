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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork uof;
        public CategoriesController(IUnitOfWork _uof)
        {
            uof = _uof;
        }
        private ObjectResult HandleServerError(Exception ex)
        {
            Console.Out.WriteLine(ex.ToString());
            return Problem("Erro ao processar a requisição!", null, StatusCodes.Status500InternalServerError);
        }
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            try
            {
                // Limitar a quantidade de categorias retornados por requisição
                List<Category> categories = await uof.CategoryRepository.Get().ToListAsync();
                if (!categories.Any())
                {
                    return Ok("Nenhuma categoria existente!");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpGet("id: int")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            try
            {
                Category? categories = await uof.CategoryRepository.GetById(c => c.CategoryId == id);
                if (categories is null)
                {
                    return NotFound("Categoria não encontrada!");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        //[HttpGet("{id: int}/products")]
        //public async Task<ActionResult<List<Product>>> GetCategoryProducts(int id)
        //{
        //    try
        //    {
        //        List<Product> categoryProducts = await uof.CategoryRepository.GetCategoryProducts(id);
        //    }
        //    catch (Exception ex)
        //    {

        //        return HandleServerError(ex);
        //    }
        //}
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            try
            {
                await uof.CategoryRepository.Add(category);
                await uof.Commit();
                return Created($"/categories/{category.CategoryId}", category);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPut("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest("O id da categoria não corresponde com id passado como parametro!");
            }
            try
            {
                uof.CategoryRepository.Update(category);
                await uof.Commit();
                Category? updatedCategory = await uof.CategoryRepository.GetById(c => c.CategoryId == id);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpDelete("int: id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Category? category = await uof.CategoryRepository.GetById(c => c.CategoryId == id);
                if (category == null)
                {
                    return NotFound("A categoria que deseja deletar não existe!");
                }
                uof.CategoryRepository.Delete(category);
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
