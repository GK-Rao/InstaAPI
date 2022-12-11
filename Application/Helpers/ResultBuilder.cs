using InstaAPI.Application.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace InstaAPI.Application.Helpers
{
    public class ResultBuilder : IResultBuilder
    {

        public IActionResult BuildClientResultGeneric<TInput>(TInput response, string status = "")
        {
            if(response == null ) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            if (status != "" && status == "failure") return new JsonResult(response) { StatusCode = StatusCodes.Status404NotFound };

            return new OkObjectResult(response);
        }

        public IActionResult BuildClientResult(BaseResponse response, string status)
        {
            if (response == null) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            if (status != "" && status == "failure") return new JsonResult(response.Exception) { StatusCode = StatusCodes.Status404NotFound };

            return new OkObjectResult(response);

        }
    }

    public interface IResultBuilder
    {
        IActionResult BuildClientResultGeneric<TInput>(TInput response, string status = "");

        IActionResult BuildClientResult(BaseResponse response, string status);
    }
}
