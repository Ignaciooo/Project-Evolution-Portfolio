using Microsoft.AspNetCore.Mvc;
using Milestone05_WebAPI.Models;
using Milestone05_WebAPI.Services;

namespace Milestone05_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ApiControllerBase
{
    private readonly GenreService _genreService;

    public GenresController(GenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Genre>), StatusCodes.Status200OK)]
    public ActionResult<List<Genre>> GetGenres()
    {
        return Ok(_genreService.GetGenres());
    }
}
