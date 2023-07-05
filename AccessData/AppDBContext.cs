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
                    .HasColumnType("date");

                entity.Property(e => e.ResetToken)
                     .HasColumnType("varchar")
                     .HasMaxLength(200);

                entity.Property(e => e.ResetTokenExpires)
                   .HasColumnType("date");

                entity.Property(e => e.PasswordReset)
                   .HasColumnType("date");

                entity.Property(e => e.Created)
                   .HasColumnType("date");

            });

            modelBuilder.Entity<UserBusiness>(entity =>
            {
                entity.HasKey(x =>x.Id);    

                entity.Property(e => e.FantasyName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.BusinessName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Slogan)
                    .HasColumnType("varchar")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnType("varchar")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Address)
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Location)
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Alias)
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Web)
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.UsersBusiness)
                      .HasForeignKey(e => e.AccountId);
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
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Stock)
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                /*
                entity.Property(e => e.ImagePath)
                    .IsRequired(false)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);
                */

                entity.HasOne(e => e.UserBusiness)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.UserBusinessId);

            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.BoxName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.BusinessName)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(e => e.UserClientEmail)
                      .HasColumnType("varchar")
                      .HasMaxLength(100);

                entity.Property(x => x.DateSale)
                      .HasColumnType("date")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Code)
                      .HasColumnType("varchar")
                      .HasMaxLength(6);

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.UserClient)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.UserClientId);
            });
        }
    }
}
