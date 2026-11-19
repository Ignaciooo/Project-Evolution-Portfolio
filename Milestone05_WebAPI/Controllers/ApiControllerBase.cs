using Microsoft.AspNetCore.Mvc;
using Milestone05_WebAPI.Services;

namespace Milestone05_WebAPI.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult ErrorResponse<T>(ServiceResult<T> result)
    {
        int status = result.Error switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        return Problem(detail: result.Message, statusCode: status);
    }
}
