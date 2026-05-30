using Domain.Entities;
using Domain.Entities.Publications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class PublicationDbContext : DbContext
    {
        public PublicationDbContext(DbContextOptions<PublicationDbContext> options)
            : base(options) { }

        public DbSet<PublicationEntity> Publications { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("pg_trgm");

            modelBuilder.Entity<PublicationEntity>().HasOne(pr => pr.Product).WithMany().HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductEntity>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<CategoryEntity>() .HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<PublicationEntity>().HasIndex(p => p.Title).HasMethod("gin").HasOperators("gin_trgm_ops");

            modelBuilder.Entity<ProductEntity>().HasIndex(pr => new { pr.CategoryId, pr.Price });

            modelBuilder.Entity<ProductEntity>().HasIndex(pr => pr.Price);

            modelBuilder.Entity<ProductEntity>(p =>
            {
                p.Property(x => x.Name).IsRequired();
                p.Property(x => x.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                p.Property(x => x.Stock).IsRequired();
            });

            modelBuilder.Entity<PublicationEntity>(p =>
            {
                p.Property(x => x.Title).IsRequired();

                p.Property(x => x.Description).IsRequired();

                p.Property(x => x.ImageUrl).IsRequired();

                

                p.Property(x => x.Status)
                    .HasConversion<string>()
                    .IsRequired();

                p.Property(x => x.UserId).IsRequired();

                p.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
            });
        }
    }
}