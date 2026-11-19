using Microsoft.AspNetCore.Mvc;
using Milestone05_WebAPI.DTOs;
using Milestone05_WebAPI.Services;

namespace Milestone05_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<UserDto> GetUser(int id)
    {
        var result = _userService.GetUser(id);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<UserDto> Register(CreateUserDto dto)
    {
        var result = _userService.Register(dto);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return CreatedAtAction(nameof(GetUser), new { id = result.Value!.Id }, result.Value);
    }
}