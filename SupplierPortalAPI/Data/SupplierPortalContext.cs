using Microsoft.EntityFrameworkCore;
using SupplierPortalAPI.Models;

namespace SupplierPortalAPI.Data
{
    public class SupplierPortalContext : DbContext
    {
        public SupplierPortalContext(DbContextOptions<SupplierPortalContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Supplier Configuration
            modelBuilder.Entity<Supplier>()
                .HasKey(s => s.SupplierId);

            // PurchaseRequest Configuration
            modelBuilder.Entity<PurchaseRequest>()
                .HasKey(pr => pr.RequestId);

            modelBuilder.Entity<PurchaseRequest>()
                .Property(pr => pr.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PurchaseRequest>()
                .Property(pr => pr.CreatedBy)
                .IsRequired();

            // Product Configuration
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .HasOne<PurchaseRequest>()
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role Configuration
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Role>()
                .HasData( // Seed default roles
                    new Role { Id = 1, Name = "Colocador" },
                    new Role { Id = 2, Name = "Aprobador" }
                );

            // User Configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role) 
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
