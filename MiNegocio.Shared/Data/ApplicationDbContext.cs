using Microsoft.EntityFrameworkCore;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;

        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<ProductWarehouse> ProductWarehouses { get; set; } = null!;

        public DbSet<ProductTransfer> ProductTransfers { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                // Relationship with Company
                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Users)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Warehouse configuration
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Warehouses)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Categories)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UnitOfMeasure configuration
            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.UnitsOfMeasure)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UnitOfMeasure)
                    .WithMany(u => u.Products)
                    .HasForeignKey(e => e.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ProductWarehouse entity
            modelBuilder.Entity<ProductWarehouse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinStock).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();

                // Composite key to ensure a product can only be in a warehouse once
                entity.HasIndex(e => new { e.ProductId, e.WarehouseId }).IsUnique();

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductWarehouses)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Warehouse)
                    .WithMany(w => w.ProductWarehouses)
                    .HasForeignKey(e => e.WarehouseId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            // ProductTransfer entity
            modelBuilder.Entity<ProductTransfer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TransferDate).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Transfers)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FromWarehouse)
                    .WithMany(w => w.OutgoingTransfers)
                    .HasForeignKey(e => e.FromWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(e => e.ToWarehouse)
                    .WithMany(w => w.IncomingTransfers)
                    .HasForeignKey(e => e.ToWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
