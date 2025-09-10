using Application.Dto;
using Application.Logic.ProductService;
using GenericRepo_Dapper.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepo_Dapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        int productsCreated = await _service.AddProductAsync(productDto);
        return Created("", new { result = (productsCreated > 0) });
    }

    [HttpGet("getall")]
    [Permission("manage_users")]
    public IActionResult GetAll() => Ok(new { message = "Danh sách sản phẩm" });

}