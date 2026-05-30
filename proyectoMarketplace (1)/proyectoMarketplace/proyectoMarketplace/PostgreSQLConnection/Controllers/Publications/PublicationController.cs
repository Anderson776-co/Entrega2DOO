using Application.Publications.DTO_s;
using Application.Publications.UseCase;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace APIDazma.Controllers.Publications
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PublicationController : ControllerBase
    {
        private readonly CreatePublicationUseCase _createUseCase;
        private readonly GetPublicationUseCase _getByFilterUseCase;
        private readonly UpdatePublicationByIdUseCase _updateUseCase;
        private readonly DeletePublicationByIdUseCase _deleteUseCase;

        private readonly IMemoryCache _cache;

        public PublicationController(CreatePublicationUseCase createUseCase, 
            GetPublicationUseCase getByFilterUseCase, UpdatePublicationByIdUseCase updateUseCase, DeletePublicationByIdUseCase deleteUseCase, IMemoryCache cache)
        {
            _createUseCase = createUseCase;
            _getByFilterUseCase = getByFilterUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
            _cache = cache;
        }

        [HttpPost("CreatePublication")]
        public async Task<IActionResult> Create([FromForm] PublicationDTO dto, IFormFile? image)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var publication = await _createUseCase.Create(
                dto,
                int.Parse(authenticatedUserId),
                image?.OpenReadStream(),
                image?.FileName
            );

            return Created("", new { message = "Publicación creada", id = publication.Id });
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string? name,[FromQuery] decimal? minPrice,[FromQuery] decimal? maxPrice,[FromQuery] string? category)
        {
            var publications = await _getByFilterUseCase.Get(name, minPrice, maxPrice, category);
            return Ok(publications);
        }

        //Implementacion de cache para la consulta de una publicacion por id, descomentar para usarla, comentar la consulta para verificar la prueba de carga y estres
        /*[HttpGet("GetOnePost/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {

            if (_cache.TryGetValue($"Publication_{id}", out PublicationResponseDTO? cachedPublication))
            {
                return Ok(cachedPublication);
            }
            else
            {
                var publication = await _getByFilterUseCase.GetPublicationById(id);

                _cache.Set($"Publication_{id}", publication, TimeSpan.FromMinutes(10));

                return Ok(publication);
            }
        }*/

        [HttpGet("GetOnePost/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var publication = await _getByFilterUseCase.GetPublicationById(id);
            return Ok(publication);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] PublicationDTO dto, IFormFile? image)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var updatedPublication = await _updateUseCase.Update(
                id,
                dto,
                int.Parse(authenticatedUserId),
                image?.OpenReadStream(),
                image?.FileName
            );

            string cacheKey = $"Publication_{id}";

            if (_cache.TryGetValue(cacheKey, out PublicationResponseDTO? _))
            {
                _cache.Remove(cacheKey);
            }

            _cache.Set(cacheKey, updatedPublication, TimeSpan.FromMinutes(10));

            return Ok(updatedPublication);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            await _deleteUseCase.Delete(id, int.Parse(authenticatedUserId));
            return Ok(new { message = "Publicación eliminada" });
        }
    }
}