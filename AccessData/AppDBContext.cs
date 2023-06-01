using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserBusiness> UserBusinesses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.VerificationToken)
                     .HasColumnType("varchar")
                     .HasMaxLength(200);

                entity.Property(e => e.VerifiedAt)
                    .HasColumnType("datetime");

                entity.Property(e => e.ResetToken)
                     .HasColumnType("varchar")
                     .HasMaxLength(200);

                entity.Property(e => e.ResetTokenExpires)
                   .HasColumnType("datetime2");

                entity.Property(e => e.PasswordReset)
                   .HasColumnType("datetime2");

                entity.Property(e => e.Created)
                   .HasColumnType("datetime2");

                entity.Property(e => e.Updated)
                   .HasColumnType("datetime2");

                entity.Property(e => e.IsActive)
                   .HasColumnName("IsActive");

            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Account)
                      .WithMany(e => e.UsersClients)
                      .HasForeignKey(e => e.AccountId);
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Name)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.HasOne(e => e.UserBusiness)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.UserBusinessId);

            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(e => e.UserClient)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.UserClientId);
            });
        }

    
    }
}
