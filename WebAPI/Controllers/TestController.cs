using Application.Dto;
using Application.Logic.CategoryService;
using Application.Logic.ProductService;
using GenericRepo_Dapper.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Utility.Helpers;
using Utility.Model.Excel;

namespace GenericRepo_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;   

        public TestController(ICategoryService categoryService, IProductService productService)   
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            int categoriesCreated = await _categoryService.Create(categoryDto);

            int productCreated = await _productService.AddProductAsync(new ProductDto() { Name = categoryDto.Name});

            if (categoriesCreated < 0) { return Fail("Error create category!", StatusCodes.Status200OK); }

            return Success(categoriesCreated);
        }

        [HttpGet("export-custom")]
        public IActionResult ExportCustomExcel()
        {
            var data = new List<List<object>>
            {
                new List<object> { 1, "Dat", 25 , DateTime.Now , 1999.99},
                new List<object> { 2, "ChatGPT", 5, "25/08/1999", 1999.99 }
            };

            var headers = new List<ExcelColumnCustom>
            {
                new ExcelColumnCustom { Header = "ID", Width = 40, BackgroundColor = Color.LightBlue, HorizontalAlignment = ExcelHorizontalAlignment.Center },
                new ExcelColumnCustom { Header = "Name", Width = 10, BackgroundColor = Color.LightGray },
                new ExcelColumnCustom { Header = "Age", Width = 30 },
                new ExcelColumnCustom { Header = "Born", Width = 30 },
                new ExcelColumnCustom { Header = "Salary", Width = 30 }
            };

            var columnFormats = new List<string>
            {
                "#",
                "@",
                "#",
                "yyyy-MM-dd",   
                "#,##0.00"  
            };

            var bytes = ExcelHelpers.ExportExcel(headers, data, lstColumnFormats: columnFormats);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "custom.xlsx");
        }

        [HttpGet("gen-query")]
        public IActionResult GenQuery()
        {
            //DateTimeHelper.GetAge(new DateTime(1997, 05, 16));
            string json = "";
            //var lstParam = JsonConvert.DeserializeObject<List<object>>(json);
            var lstParam = new List<object?>
            {
                new DateTime(2025, 8, 30, 0, 0, 0),   // 8/30/2025 12:00:00 AM
                new DateTime(2025, 9, 6, 0, 0, 0),    // 9/6/2025 12:00:00 AM
                "1255",                                 // int
                null,                                 // empty
                null,                                 // empty
                null,                                 // empty
                null,                                 // empty
                1,                                    // int
                1,                                    // int
                10                                    // int
            };

            string query = GenQueryStoredHelpers.GenQuerySearch(lstParam, "purchasing.pom_sellinestimated_srh");

            return Success(query);
        }

    }
}
