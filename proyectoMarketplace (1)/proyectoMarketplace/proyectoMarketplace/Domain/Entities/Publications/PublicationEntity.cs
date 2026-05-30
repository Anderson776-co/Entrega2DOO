using Domain.Entities.Publications;

namespace Domain.Entities
{
    public class PublicationEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public PublicationStatus Status { get; set; } = PublicationStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
        public int UserId { get; set; }
    }
}