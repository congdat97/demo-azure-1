using Application.Dto;
using Application.Logic.CategoryService;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepo_Dapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : BaseController
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
    {
        int categoriesCreated = await _service.Create(categoryDto);

        if (categoriesCreated < 0) { return Fail("Error create category!", StatusCodes.Status200OK); }

        return Success(categoriesCreated);
    }

    [HttpPost("/test")]
    public async Task<IActionResult> CreateTestTransaction([FromBody] CategoryDto categoryDto, [FromQuery] ProductDto productDto)
    {
        try
        {
            await _service.CreateTestTransaction(categoryDto, productDto);
            return Success("");
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpPost("AddCategoryAsync")]
    public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryDto categoryDto) 
    {
        int categoriesCreated = await _service.AddCategoryAsync(categoryDto);

        if (categoriesCreated < 0) { return Fail("Error create category!", StatusCodes.Status200OK); }

        return Success(categoriesCreated);
    }
}