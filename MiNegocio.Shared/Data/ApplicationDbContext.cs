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
        }
    }
}
