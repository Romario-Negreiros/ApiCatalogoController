using ApiCatalogoController.DTOs;
using ApiCatalogoController.Models;
using ApiCatalogoController.Pagination;
using ApiCatalogoController.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogoController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork uof;
        private readonly IMapper mapper;
        public CategoriesController(IUnitOfWork _uof, IMapper _mapper)
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
        public async Task<ActionResult> GetCategories([FromQuery] PaginationParameters paginationParameters)
        {
            try
            {
                PagedList<Category> categories = await uof.CategoryRepository.GetCategories(paginationParameters);
                if (!categories.Any())
                {
                    return NotFound("Nenhuma categoria existente!");
                }

                var metadata = new
                {
                    categories.TotalCount,
                    categories.PageSize,
                    categories.CurrentPage,
                    categories.TotalPages,
                    categories.HasNext,
                    categories.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                List<CategoryDTO> categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
                return Ok(categoriesDTO);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
        //    try
        //    {
        //        // Limitar a quantidade de categorias retornados por requisição
        //        List<Category> categories = await uof.CategoryRepository.Get().ToListAsync();
        //        if (!categories.Any())
        //        {
        //            return Ok("Nenhuma categoria existente!");
        //        }
        //        List<CategoryDTO> categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
        //        return Ok(categories);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleServerError(ex);
        //    }
        //}
        [HttpGet("id: int")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Category? category = await uof.CategoryRepository.GetCategoryProducts(id);
                if (category is null)
                {
                    return NotFound("Categoria não encontrada!");
                }
                CategoryDTO categoryDTO = mapper.Map<CategoryDTO>(category);
                return Ok(categoryDTO);
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(CategoryDTO categoryDTO)
        {
            try
            {
                Category category = mapper.Map<Category>(categoryDTO);
                await uof.CategoryRepository.Add(category);
                await uof.Commit();
                CategoryDTO updatedCategoryDTO = mapper.Map<CategoryDTO>(category);
                return Created($"/categories/{category.CategoryId}", updatedCategoryDTO);
            }
            catch (Exception ex)
            {
                return HandleServerError(ex);
            }
        }
        [HttpPut("id: int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
            {
                return BadRequest("O id da categoria não corresponde com id passado como parametro!");
            }
            try
            {
                Category category = mapper.Map<Category>(categoryDTO);
                uof.CategoryRepository.Update(category);
                await uof.Commit();
                Category? updatedCategory = await uof.CategoryRepository.GetById(c => c.CategoryId == id);
                return Ok(mapper.Map<CategoryDTO>(updatedCategory));
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
