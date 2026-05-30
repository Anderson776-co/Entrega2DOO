using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<OrderEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderEntity>(o =>
            {
                o.Property(p => p.UserId).IsRequired();
                o.Property(p => p.PublicationId).IsRequired();
                o.Property(p => p.Status).HasConversion<string>().IsRequired();
                o.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                o.Property(p => p.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
            });
        }
    }
}
