using Application.Publications.DTO_s;
using Application.Publications.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services.Publications;

namespace Application.Publications.UseCase
{
    public class UpdatePublicationByIdUseCase
    {
        private readonly PublicationService _publicationService;
        private readonly IImageService _imageService;

        public UpdatePublicationByIdUseCase(PublicationService publicationService, IImageService imageService)
        {
            _publicationService = publicationService;
            _imageService = imageService;
        }

        public async Task<PublicationResponseDTO> Update(int publicationId, PublicationDTO dto, int userId, Stream? imageStream, string? fileName)
        {
            var imageUrl = "";
            if (imageStream != null && fileName != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(fileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                    throw new ValidationException("Solo se permiten imágenes JPG, PNG o WEBP.");

                if (imageStream.Length > 5 * 1024 * 1024)
                    throw new ValidationException("La imagen no puede superar 5MB.");

                imageUrl = await _imageService.SaveImageAsync(imageStream, fileName);
            }

            var publicationEntity = PublicationDTO.FromDTOToEntity(dto, imageUrl, userId);
            var productEntity = PublicationDTO.createProduct(dto);

            var updatedEntity = await _publicationService.Update(publicationEntity, productEntity);
            return PublicationResponseDTO.FromEntityToDTO(updatedEntity);
        }
    }
}
