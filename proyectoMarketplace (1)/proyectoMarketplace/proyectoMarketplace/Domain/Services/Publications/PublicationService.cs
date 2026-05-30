using Domain.Entities;
using Domain.Entities.Publications;
using Domain.Exceptions;
using Domain.Ports;

namespace Domain.Services.Publications
{
    public class PublicationService
    {
        private readonly IPublicationRepository _publicationRepository;

        public PublicationService(IPublicationRepository publicationRepository)
        {
            _publicationRepository = publicationRepository;
        }

        public async Task<PublicationEntity> Create(PublicationEntity publication, ProductEntity product)
        {
            if (product.Price <= 0)
                throw new ValidationException("El precio debe ser mayor a 0.");

            if (product.Stock < 0)
                throw new ValidationException("El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(publication.Title))
                throw new ValidationException("El título es obligatorio.");
            if (string.IsNullOrWhiteSpace(publication.Description))
                throw new ValidationException("La descripción es obligatoria.");

            var createdPublication = await _publicationRepository.Create(publication, product);
            var createdProduct = await _publicationRepository.GetProductById(createdPublication.ProductId)
                                 ?? throw new InvalidOperationException("No se pudo obtener el producto recién creado.");
            return createdPublication;
        }

        public async Task<PublicationEntity> Update(PublicationEntity publication, ProductEntity product)
        {
            var existing = await _publicationRepository.GetById(publication.Id)
                ?? throw new NonFoundException("Publicación no encontrada.");

            if (existing.UserId != publication.UserId)
                throw new UnauthorizedException("No tienes permiso para editar esta publicación.");

            existing.Title = publication.Title;
            existing.Description = publication.Description;
            existing.Product.Price = product.Price;
            existing.Product.Stock = product.Stock;
            existing.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(publication.ImageUrl))
                existing.ImageUrl = publication.ImageUrl;

            return await _publicationRepository.Update(existing);
        }

        public async Task Delete(int publicationId, int userId)
        {
            var existing = await _publicationRepository.GetById(publicationId)
                ?? throw new NonFoundException("Publicación no encontrada.");

            if (existing.UserId != userId)
                throw new UnauthorizedException("No tienes permiso para eliminar esta publicación.");

            await _publicationRepository.Delete(publicationId);
        }

        public async Task<List<PublicationEntity>> SearchByParameters(string? name, decimal? minPrice, decimal? maxPrice, string? category)
        {
            if (string.IsNullOrWhiteSpace(name) && !minPrice.HasValue && !maxPrice.HasValue && string.IsNullOrWhiteSpace(category))
                throw new ValidationException("Debe proporcionar al menos un criterio de búsqueda.");

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                throw new ValidationException("El precio mínimo no puede ser mayor al máximo.");

            return await _publicationRepository.SearchByParameters(name?.Trim(), minPrice, maxPrice, category);
        }

        public async Task<PublicationEntity> GetPublicationById(int id)
        {
            var publication = await _publicationRepository.GetById(id);
            if (publication == null)
                throw new NonFoundException("Publicación no encontrada.");
            return publication;
        }
    }
}
