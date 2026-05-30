
namespace Domain.Entities.Publications
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
