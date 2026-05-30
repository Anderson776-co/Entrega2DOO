using Domain.Entities;
using Domain.Entities.Publications;

namespace Domain.Ports
{
    public interface IPublicationRepository
    {
        Task<PublicationEntity> Create(PublicationEntity publication, ProductEntity product);
        Task<PublicationEntity?> GetById(int id);
        Task<List<PublicationEntity>> SearchByParameters(string? name, decimal? minPrice, decimal? maxPrice, string? category);
        Task<PublicationEntity> Update(PublicationEntity publication);
        Task Delete(int id);
        Task<ProductEntity?> GetProductById(int id);
        Task<bool> IsPublicationAvailable(int publicationId);
        Task<bool> ExistsPublicationById(int id);
        Task<decimal> GetPriceById(int publicationId);
    }
}
