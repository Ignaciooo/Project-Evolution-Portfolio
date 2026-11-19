using Milestone05_WebAPI.Models;
using Milestone05_WebAPI.Repositories;

namespace Milestone05_WebAPI.Services;

public class GenreService
{
    private readonly GenreRepository _genreRepository;

    public GenreService(GenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public List<Genre> GetGenres()
    {
        return _genreRepository.GetAll();
    }
}