using Application.Publications.DTO_s;
using Domain.Entities.Publications;
using Domain.Services.Publications;

namespace Application.Publications.UseCase
{
    public class GetPublicationUseCase
    {
        private readonly PublicationService _publicationService;

        public GetPublicationUseCase(PublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        public async Task<List<PublicationResponseDTO>> Get(string? name, decimal? minPrice, decimal? maxPrice, string? category)
        {
            return (await _publicationService.SearchByParameters(name, minPrice, maxPrice, category))
                .Select(PublicationResponseDTO.FromEntityToDTO)
                .ToList();
        }

        public async Task<PublicationResponseDTO> GetPublicationById(int id)
        {
            var publication = await _publicationService.GetPublicationById(id);
            return PublicationResponseDTO.FromEntityToDTO(publication);
        }
    }
}