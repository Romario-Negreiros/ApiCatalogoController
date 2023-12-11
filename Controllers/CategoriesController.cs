using ApiCatalogoController.Context;
using ApiCatalogoController.Models;
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
        private readonly AppDbContext ctx;
        public CategoriesController(AppDbContext _ctx)
        {
            ctx = _ctx;
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
                List<Category> categories = await ctx.Categories.AsNoTracking().ToListAsync();
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
                Category? categories = await ctx.Categories.FindAsync(id);
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
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            try
            {
                ctx.Categories.Add(category);
                await ctx.SaveChangesAsync();
                return Created($"/categories/{category.CategoryId}", category);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPut("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Category>> Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest("O id da categoria não corresponde com id passado como parametro!");
            }
            try
            {
                Category? categoryInDb = await ctx.Categories.FindAsync(id);
                if (categoryInDb == null)
                {
                    return NotFound("A categoria que deseja atualizar não foi encontrada!");
                }
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                await ctx.SaveChangesAsync();
                return Ok(categoryInDb);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpDelete("int: id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            try
            {
                Category? category = await ctx.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound("A categoria que deseja deletar não existe!");
                }
                ctx.Categories.Remove(category);
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
