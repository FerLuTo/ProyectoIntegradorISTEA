using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessData
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserBusiness> UserBusinesses { get; set; }
        public DbSet<UserClient> UserClients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }


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
                    .HasColumnType("datetime2");

                entity.Property(e => e.ResetToken)
                     .HasColumnType("varchar")
                     .HasMaxLength(200);

                entity.Property(e => e.ResetTokenExpires)
                   .HasColumnType("datetime2");

                entity.Property(e => e.PasswordReset)
                   .HasColumnType("datetime2");

                entity.Property(e => e.Created)
                   .HasColumnType("datetime2");

            });

            modelBuilder.Entity<UserBusiness>(entity =>
            {
                entity.HasKey(x =>x.Id);    

                entity.Property(e => e.FantasyName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.BusinessName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.Slogan)
                    .HasColumnType("varchar")
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasColumnType("varchar")
                    .HasMaxLength(200);

                entity.Property(e => e.Address)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.Location)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

                entity.Property(e => e.Alias)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

                entity.Property(e => e.Web)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.HasKey(x => x.Id);
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

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.ImagePath)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.HasOne(e => e.UserBusiness)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.UserBusinessId);

            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.BusinessName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.UserClientEmail)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(x => x.DateSale)
                      .HasColumnType("datetime2")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Code)
                      .HasColumnType("varchar");

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.UserClient)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.UserClientId);
            });
        }
    }
}
