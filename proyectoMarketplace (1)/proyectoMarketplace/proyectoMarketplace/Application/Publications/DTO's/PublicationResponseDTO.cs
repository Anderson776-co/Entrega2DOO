using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Publications.DTO_s
{
    public class PublicationResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public static PublicationResponseDTO FromEntityToDTO(PublicationEntity entity)
        {
            return new PublicationResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Product.Price,
                Stock = entity.Product.Stock,
                Category = entity.Product.Category.Name,
                ImageUrl = entity.ImageUrl,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
