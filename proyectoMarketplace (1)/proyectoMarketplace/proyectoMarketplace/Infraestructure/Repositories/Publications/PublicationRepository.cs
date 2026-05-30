using Domain.Entities;
using Domain.Entities.Publications;
using Domain.Ports;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Publications
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly PublicationDbContext _context;

        public PublicationRepository(PublicationDbContext context)
        {
            _context = context;
        }

        public async Task<PublicationEntity> Create(PublicationEntity publication, ProductEntity product)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == product.Category.Name.ToLower());

            if (existingCategory != null)
            {
                product.CategoryId = existingCategory.Id;
                product.Category = null; 
            }
            else
            {
                _context.Categories.Add(product.Category);
                await _context.SaveChangesAsync(); 
                product.CategoryId = product.Category.Id;
                product.Category = null;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); 

         
            publication.ProductId = product.Id; 
            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();

            return publication;
        }

        public async Task<PublicationEntity?> GetById(int id)
        {
            return await _context.Publications
                .Include(p => p.Product)           
                    .ThenInclude(p => p.Category)  
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PublicationEntity>> SearchByParameters(string? name, decimal? minPrice, decimal? maxPrice, string? category)
        {
            var query = _context.Publications
                .Include(p => p.Product)
                    .ThenInclude(p => p.Category)
                .Where(p => p.Status == PublicationStatus.Active);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => EF.Functions.ILike(p.Title, $"%{name}%"));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => EF.Functions.ILike(p.Product.Category.Name, $"%{category}%"));

            if (minPrice.HasValue)
                query = query.Where(p => p.Product.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Product.Price <= maxPrice.Value);

            return await query.ToListAsync();
        }

        public async Task<PublicationEntity> Update(PublicationEntity publication)
        {
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task Delete(int id)
        {
            var publication = _context.Publications.FirstOrDefault(p => p.Id == id);
            if (publication != null)
            {
                publication.Status = PublicationStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductEntity?> GetProductById(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<bool> IsPublicationAvailable(int publicationId)
        {
            return _context.Publications.AnyAsync(p => p.Id == publicationId && p.Status == PublicationStatus.Active);
        }

        public Task<bool> ExistsPublicationById(int id)
        {
            return _context.Publications.AnyAsync(p => p.Id == id);
        }

        public Task<decimal> GetPriceById(int publicationId)
        {
            return _context.Publications
                .Where(p => p.Id == publicationId)
                .Select(p => p.Product.Price)
                .FirstOrDefaultAsync();
        }
    }
}