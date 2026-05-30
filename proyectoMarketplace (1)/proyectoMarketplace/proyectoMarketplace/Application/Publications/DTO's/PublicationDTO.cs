using Domain.Entities;
using Domain.Entities.Publications;
using System.ComponentModel.DataAnnotations;

namespace Application.Publications.DTO_s
{
    public class PublicationDTO
    {
        [Required] public string Title { get; set; } = "";
        [Required] public string ProductName { get; set; } = "";
        [Required] public string Description { get; set; } = "";
        [Required] public decimal Price { get; set; }
        [Required] public int Stock { get; set; }
        [Required] public string Category { get; set; } = "";

        public static PublicationEntity FromDTOToEntity(PublicationDTO publicationDTO, string imageUrl, int userId)
        {
            return new PublicationEntity
            {
                Title = publicationDTO.Title.Trim(),
                Description = publicationDTO.Description.Trim(),
                ImageUrl = imageUrl,
                UserId = userId
            };
        }

        public static ProductEntity createProduct(PublicationDTO publicationDTO)
        {
            return new ProductEntity
            {
                Name = publicationDTO.ProductName.Trim(),
                Price = publicationDTO.Price,
                Stock = publicationDTO.Stock,
                Category = new CategoryEntity { Name = publicationDTO.Category.Trim() }
            };
        }
    }
 }
