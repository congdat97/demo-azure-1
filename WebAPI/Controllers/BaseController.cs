using GenericRepo_Dapper.Common;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepo_Dapper.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = "Success", string messageDetail = "") =>
            new ResponseResult<T>(new ApiResponse<T>(true, message, messageDetail, data));

        protected IActionResult Fail(string message, int statusCode = 400, string messageDetail = "") =>
            new ResponseResult<string>(new ApiResponse<string>(false, message, messageDetail), statusCode);
    }
}
