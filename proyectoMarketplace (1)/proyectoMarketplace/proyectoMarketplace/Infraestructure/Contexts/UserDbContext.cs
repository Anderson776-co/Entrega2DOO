using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Business;
namespace Infrastructure.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<MailingAddressEntity> MailingAddresses { get; set; }
        public DbSet<BusinessEntity> Businesses { get; set; }
        public DbSet<InvitationCodeEntity> InvitationCodes { get; set; }
        public DbSet<RevokedTokenEntity> RevokedTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Phone).IsUnique();

            modelBuilder.Entity<UserEntity>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<UserEntity>().HasOne(u => u.Business).WithMany(e => e.Users).HasForeignKey(u => u.BusinessId).IsRequired(false);
            modelBuilder.Entity<MailingAddressEntity>().HasOne(u => u.User).WithMany(u => u.MailingAddresses).HasForeignKey(s => s.UserId);
            modelBuilder.Entity<BusinessEntity>().HasOne<UserEntity>().WithMany().HasForeignKey(b => b.AdminUserId);
            modelBuilder.Entity<InvitationCodeEntity>().HasOne(i => i.Business).WithMany().HasForeignKey(i => i.BusinessId);
            modelBuilder.Entity<InvitationCodeEntity>().HasOne(i => i.Role).WithMany().HasForeignKey(i => i.RoleId);
            modelBuilder.Entity<InvitationCodeEntity>().HasOne(i => i.User).WithMany().HasForeignKey(i => i.UserId).IsRequired(false);
            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = 1, Name = "StoreAdmin" },
                new RoleEntity { Id = 2, Name = "Assistant" },
                new RoleEntity { Id = 3, Name = "NaturalPerson" },
                new RoleEntity { Id = 4, Name = "PlatformAdmin" },
                new RoleEntity { Id = 5, Name = "Moderator" });
        }
    }
}
  