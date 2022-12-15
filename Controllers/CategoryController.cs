using Blogao.Context;
using Blogao.Models;
using Blogao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLogao.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
    {
        var categories = await context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Categoria nao encontrada!");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno do servidor!");
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateCategoryViewModel model,
        [FromServices] AppDbContext context)
    {
        try
        {
            var category = new Category
            {
                Id = 0,
                Posts = null,
                Name = model.Name,
                Slug = model.Slug.ToLower(),
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Nao foi possivel incluir a categoria.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha interna no servidor!");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model, [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Nao foi atualizar a categoria");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno do servidor!");
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Nao foi possivel excluir a categoria!");
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno do servidor!");
        }
    }
}